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
	Measurer currentMeasurer;

	[SerializeField] GameObject[] stages;
	GameObject[] loadedStages = new GameObject[2];
	Transform lastStageEnd;
	int loadedIndex = 0;

	// Use this for initialization
	void Start() {
		lastStageEnd = transform;
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		signText = GameObject.Find("Sign").GetComponent<Text>();
		currentTimeText = GameObject.Find("Timer").GetComponent<Text>();
		recordText = GameObject.Find("Record").GetComponent<Text>();
		diffText = GameObject.Find("Diff").GetComponent<Text>();

		LoadStage();
		LoadStage(); // Prefetching
	}

	// Update is called once per frame
	void Update() { }

	public void LoadStage() {
		if (stages[loadedIndex] == null)
			return;

		if (loadedIndex > 2) {
			Destroy(loadedStages[0]);
		}

		loadedStages[0] = loadedStages[1];

		var newStage = Instantiate(stages[loadedIndex], lastStageEnd.position,
			Quaternion.identity);
		foreach (Transform child in newStage.transform) {
			if (child.tag == "Goal")
				lastStageEnd = child;
			else if (child.tag == "Finish") {
				var prevMeasurer = currentMeasurer;
				currentMeasurer = child.gameObject.GetComponent<Measurer>();
				if (loadedIndex > 0) {
					prevMeasurer.Init(currentTimeText, recordText, diffText);
					prevMeasurer.MeasureStart();
				}
			}
		}
		loadedStages[1] = newStage;
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
		// TODO: save highscores
	}
}