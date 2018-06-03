using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	PlayerController player;
	Text signText;
	Text currentTimeText;
	Text recordText;
	Text diffText;
	Measurer[] loadedMeasures = new Measurer[2];

	[SerializeField] GameObject[] stages;
	GameObject[] loadedStages = new GameObject[3];
	Vector3 lastStageEnd;
	int loadedIndex = 0;

	GameStorageManager saver;
	int playingIndex = 0;
	float[] tmpScores = new float[5];

	SceneLoader loader;

	// Use this for initialization
	void Start() {
		var builderObj = GameObject.Find("StageBuilder");
		if (builderObj != null) {
			var builder = builderObj.GetComponent<StageBuilder>();
			if (builder != null)
				stages = builder.GetComponent<StageBuilder>().Fetch();
		}

		lastStageEnd = transform.position;
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		signText = GameObject.Find("Sign").GetComponent<Text>();
		currentTimeText = GameObject.Find("Timer").GetComponent<Text>();
		recordText = GameObject.Find("Record").GetComponent<Text>();
		diffText = GameObject.Find("Diff").GetComponent<Text>();
		saver = GetComponent<GameStorageManager>();
		loader = builderObj.GetComponent<SceneLoader>();

		saver.Load(out tmpScores);

		LoadStage();
		LoadStage(); // Prefetching
	}

	// Update is called once per frame
	void Update() { }

	public void LoadStage() {
		if (stages[loadedIndex] == null)
			return;

		if (loadedIndex >= 3) {
			Destroy(loadedStages[0]);
		}

		loadedStages[0] = loadedStages[1];
		loadedStages[1] = loadedStages[2];

		var newStage = Instantiate(stages[loadedIndex], lastStageEnd,
			Quaternion.identity);
		foreach (Transform child in newStage.transform) {
			if (child.tag == "Goal") {
				lastStageEnd = child.position;
			}
			else if (child.tag == "Finish") {
				loadedMeasures[0] = loadedMeasures[1];
				loadedMeasures[1] = child.gameObject.GetComponent<Measurer>();
				if (loadedMeasures[0] != null) {
					loadedMeasures[0].Init(currentTimeText, recordText, diffText);
					loadedMeasures[0].MeasureStart(tmpScores[playingIndex]);
				}
			}
		}
		loadedStages[2] = newStage;
		signText.text = "AREA: " + loadedIndex.ToString();
		++loadedIndex;
	}

	public void Respawn() {
		player.Respawn();
	}

	public PlayerController GetPlayer() {
		return player;
	}

	public void SetRespawn(Vector3 pos) {
		player.SetRespawn(pos);
	}

	public void Goal(float score) {
		LoadStage();

		if (tmpScores[playingIndex] > score)
			tmpScores[playingIndex] = score;
		saver.Save(tmpScores);
		++playingIndex;
	}

	public void ToggleMeasurer() {
		if (loadedMeasures[0] == null) {
			loader.LoadScene("MainMenu");
			return;
		}

		if (loadedMeasures[0].IsMeasuring()) {
			Time.timeScale = 0;
			loadedMeasures[0].MeasureStop();
		}
		else {
			Time.timeScale = 1;
			loadedMeasures[0].MeasureResume();
		}
	}
}