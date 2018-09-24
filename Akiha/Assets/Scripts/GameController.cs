using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(GameStorageManager))]
public class GameController : MonoBehaviour {
	PlayerController player;
	Text signText;
	Text currentTimeText;
	Text recordText;
	Text diffText;
	Queue<Measurer> loadedMeasures = new Queue<Measurer>();

	[SerializeField] GameObject[] stages;
	Queue<GameObject> loadedStages = new Queue<GameObject>();
	Vector3 lastStageEnd;
	int loadedIndex = 0;

	GameStorageManager saver;
	int playingIndex = 0;
	float[] tmpScores = new float[5];

	[SerializeField] Canvas wholeCanvas;
	[SerializeField] Image fader;
	WaitViewer waitViewer;

	Vector3 stageStartPos;

	[SerializeField] UnityEvent pauseEvent;
	[SerializeField] UnityEvent continueEvent;

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
		stageStartPos = player.gameObject.transform.position;
		signText = GameObject.Find("Sign").GetComponent<Text>();
		currentTimeText = GameObject.Find("Timer").GetComponent<Text>();
		recordText = GameObject.Find("Record").GetComponent<Text>();
		diffText = GameObject.Find("Diff").GetComponent<Text>();
		waitViewer = GameObject.Find("ReadyContainer").GetComponent<WaitViewer>();
		saver = GetComponent<GameStorageManager>();

		saver.Load(out tmpScores);

		Time.timeScale = 0f;
		LoadStage();
		LoadStage(); // Prefetching

		waitViewer.ViewGetReady(() => {
			continueEvent.Invoke();
		});
	}

	void InitMeasurer() {
		if (loadedMeasures.Peek() == null) {
			return;
		}
		loadedMeasures.Peek().Init(currentTimeText, recordText, diffText, this.Goal);
		loadedMeasures.Peek().MeasureStart(tmpScores[playingIndex]);
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
			return;
		}

		if (loadedIndex >= 3) {
			Destroy(loadedStages.Peek());
			loadedStages.Dequeue();
		}

		var newStage = Instantiate(stages[loadedIndex], lastStageEnd,
			Quaternion.identity);
		foreach (Transform child in newStage.transform) {
			if (child.tag == "End") {
				lastStageEnd = child.position;
			} else if (child.tag == "Finish") {
				if (loadedMeasures.Count > 2) {
					loadedMeasures.Dequeue();
				}
				loadedMeasures.Enqueue(child.gameObject.GetComponent<Measurer>());
				InitMeasurer();
			}
		}
		loadedStages.Enqueue(newStage);
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
		playerPos.y += 2;
		player.SetRespawn(playerPos);
		stageStartPos = playerPos;
		LoadStage();

		if (tmpScores[playingIndex] > score)
			tmpScores[playingIndex] = score;
		saver.Save(tmpScores);
		++playingIndex;
	}

	public void ToggleMeasurer() {
		if (loadedMeasures.Peek() == null) {
			GoToMainMenu();
			return;
		}

		if (loadedMeasures.Peek().IsMeasuring()) {
			pauseEvent.Invoke();
			Time.timeScale = 0;
			loadedMeasures.Peek().MeasureStop();
		} else {
			waitViewer.ViewGetReady(() => {
				continueEvent.Invoke();
			});
			loadedMeasures.Peek().MeasureResume();
		}
	}

	public void GoToMainMenu() {
		SceneManager.LoadScene("MainMenu");
	}

	public void GoToStageSelect() {
		SceneManager.LoadScene("StageSelect");
	}

	public void Restart() {
		player.SetRespawn(stageStartPos);
		player.Respawn();
		waitViewer.ViewGetReady();
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