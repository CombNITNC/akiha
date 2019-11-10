using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#pragma warning disable 0649
[RequireComponent(typeof(HUDView))]
public class GameController : MonoBehaviour {
	PlayerController player;
	Queue<Measurer> loadedMeasures = new Queue<Measurer>();

	[SerializeField] GameObject[] stages = new GameObject[5];
	Queue<GameObject> loadedStages = new Queue<GameObject>();
	Vector3 lastStageEnd;
	int loadedIndex = 0;

	GameStorageManager saver;
	int playingIndex = 0;
	float[] tmpScores;

	[SerializeField] Canvas wholeCanvas = null;
	[SerializeField] Image fader = null;
	WaitViewer waitViewer;

	Vector3 stageStartPos;

	[SerializeField] UnityEvent pauseEvent = null;
	[SerializeField] UnityEvent continueEvent = null;

	HUDView hud;

	// Use this for initialization
	void Start() {
		var god = GameObject.FindWithTag("God");
		if (god != null) {
			var gameGod = god.GetComponent<GameGod>();
			if (gameGod != null)
				stages = gameGod.GetStory().Fetch();
		}
		tmpScores = new float[stages.Length];

		lastStageEnd = transform.position;
		player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
		stageStartPos = player.gameObject.transform.position;
		waitViewer = GameObject.Find("ReadyContainer").GetComponent<WaitViewer>();
		saver = GetComponent<GameStorageManager>();
		if (saver == null)
			saver = gameObject.AddComponent<GameStorageManager>();
		hud = GetComponent<HUDView>();

		saver.Load(out tmpScores);
		hud.SetScores(tmpScores);

		Time.timeScale = 0f;
		StartCoroutine(PrefetchStage());

		waitViewer.ViewGetReady(() => {
			continueEvent.Invoke();
		});
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

	private void PreProcessStage(GameObject newStage) {
		foreach (Transform child in newStage.transform) {
			if (child.tag == "End") {
				lastStageEnd = child.position;
			} else if (child.tag == "Finish") {
				var m = Measurer.AttachMeasure(child.gameObject, hud.SetTime, this.Goal);
				loadedMeasures.Enqueue(m);
			}
		}
	}

	private IEnumerator PrefetchStage() {
		yield return StartCoroutine(LoadStage());
		yield return StartCoroutine(LoadStage());
	}

	public IEnumerator LoadStage() {
		if (stages[loadedIndex] == null) {
			yield break;
		}

		if (loadedIndex >= 3) {
			Destroy(loadedStages.Dequeue());
		}

		var newStage = Instantiate(stages[loadedIndex], lastStageEnd,
			Quaternion.identity);
		PreProcessStage(newStage);
		loadedStages.Enqueue(newStage);
		++loadedIndex;

		if (2 <= loadedIndex) {
			hud.SetArea(loadedIndex - 2);
			loadedMeasures.Peek().MeasureStart();
		}

		yield return newStage;
	}

	private bool Goal(float score) {
		var playerPos = player.gameObject.transform.position;
		playerPos.y += 2;
		player.SetRespawn(playerPos);
		stageStartPos = playerPos;
		StartCoroutine(LoadStage());

		var isScoreUpdated = tmpScores[playingIndex] > score;
		if (isScoreUpdated)
			tmpScores[playingIndex] = score;
		saver.Save(tmpScores);
		++playingIndex;
		return isScoreUpdated;
	}

	public void ToggleMeasurer() {
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
		Time.timeScale = 1;
		SceneManager.LoadScene("MainMenu");
	}

	public void GoToStageSelect() {
		Time.timeScale = 1;
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