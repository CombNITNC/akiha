using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(GameStorageManager))]
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

	[SerializeField] Canvas wholeCanvas;
	[SerializeField] Image fader;
	WaitViewer waitViewer;

	// Use this for initialization
	void Start() {
		var god = GameObject.FindWithTag("God");
		if (god != null) {
			var gameGod = god.GetComponent<GameGod>();
			if (gameGod != null)
				stages = gameGod.GetStory().Fetch();
		}

		lastStageEnd = transform.position;
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		signText = GameObject.Find("Sign").GetComponent<Text>();
		currentTimeText = GameObject.Find("Timer").GetComponent<Text>();
		recordText = GameObject.Find("Record").GetComponent<Text>();
		diffText = GameObject.Find("Diff").GetComponent<Text>();
		waitViewer = GameObject.Find("ReadyContainer").GetComponent<WaitViewer>();
		saver = GetComponent<GameStorageManager>();

		saver.Load(out tmpScores);

		LoadStage();
		LoadStage(); // Prefetching

		waitViewer.ViewGetReady();
	}

	void InitMeasurer() {
		if (loadedMeasures[0] == null) {
			if (loadedMeasures[1] == null) {
				return;
			}
			loadedMeasures[0] = loadedMeasures[1];
			loadedMeasures[1] = null;
		}
		loadedMeasures[0].Init(currentTimeText, recordText, diffText);
		loadedMeasures[0].MeasureStart(tmpScores[playingIndex]);
	}

	IEnumerator FinishWork() {
		float count = 0;
		while (count < 2f) {
			count += Time.deltaTime;
			var c = fader.color;
			c.a = count / 2f;
			fader.color = c;
			yield return null;
		}
		GoToMainMenu();
		yield break;
	}

	public void LoadStage() {
		if (stages[loadedIndex] == null) {
			loadedStages[0] = loadedStages[1];
			loadedStages[1] = loadedStages[2];
			loadedStages[2] = null;
			loadedMeasures[0] = loadedMeasures[1];
			loadedMeasures[1] = null;
			return;
		}

		if (loadedIndex >= 3) {
			Destroy(loadedStages[0]);
		}

		loadedStages[0] = loadedStages[1];
		loadedStages[1] = loadedStages[2];

		var newStage = Instantiate(stages[loadedIndex], lastStageEnd,
			Quaternion.identity);
		foreach (Transform child in newStage.transform) {
			if (child.tag == "End") {
				lastStageEnd = child.position;
			}
			else if (child.tag == "Finish") {
				loadedMeasures[0] = loadedMeasures[1];
				loadedMeasures[1] = child.gameObject.GetComponent<Measurer>();
				InitMeasurer();
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
		var playerPos = player.gameObject.transform.position;
		playerPos.y += 20;
		player.SetRespawn(playerPos);
		LoadStage();

		if (tmpScores[playingIndex] > score)
			tmpScores[playingIndex] = score;
		saver.Save(tmpScores);
		++playingIndex;
	}

	public void ToggleMeasurer() {
		if (loadedMeasures[0] == null) {
			GoToMainMenu();
			return;
		}

		if (loadedMeasures[0].IsMeasuring()) {
			Time.timeScale = 0;
			loadedMeasures[0].MeasureStop();
		}
		else {
			waitViewer.ViewGetReady();
			loadedMeasures[0].MeasureResume();
		}
	}

	public void GoToMainMenu() {
		SceneManager.LoadScene("MainMenu");
	}

	public void GoToStageSelect() {
		SceneManager.LoadScene("StageSelect");
	}

	public void NotifyChangedSettings() {
		player.SetControlMode((ControlMode) PlayerPrefs.GetInt("ControlMode"));
	}

	public void Finish(Vector3 finishPos) {
		wholeCanvas.enabled = false;
		player.Finish(finishPos);
		StartCoroutine(FinishWork());
	}
}