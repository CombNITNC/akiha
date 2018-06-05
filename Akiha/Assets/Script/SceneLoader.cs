using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {
	static bool made = false;
	bool isTransitioning = false;
	float alpha = 0.0f;

	[SerializeField] string entry = "MainMenu";
	[SerializeField] Text text;
	[SerializeField] float speedPerSec = 2f;
	[SerializeField] UnityEvent willTransition;
	[SerializeField] UnityEvent didTransition;

	void Awake() {
		if (!made) {
			made = true;
			DontDestroyOnLoad(gameObject);
		}
	}

	void Start() {
		SceneManager.LoadScene(entry);
		text.raycastTarget = false;
		isTransitioning = false;
		alpha = 0.0f;
	}

	void Update() {
		if (isTransitioning) {
			if (alpha < 1.0f)
				alpha += Time.time * speedPerSec;
		}
		else {
			if (0f < alpha)
				alpha -= Time.time * speedPerSec;
		}

		if (text == null)
			return;

		var c = text.color;
		c.a = alpha;
		text.color = c;
	}

	IEnumerator TransitionWork(string name) {
		isTransitioning = true;
		text.raycastTarget = true;
		var prevScene = SceneManager.GetActiveScene();
		willTransition.Invoke();
		yield return SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

		var nextScene = SceneManager.GetSceneByName(name);
		SceneManager.MoveGameObjectToScene(text.gameObject, nextScene);
		SceneManager.SetActiveScene(nextScene);
		yield return SceneManager.UnloadSceneAsync(prevScene);

		text.raycastTarget = false;
		isTransitioning = false;
		didTransition.Invoke();
		yield break;
	}

	public void LoadScene(string name) {
		if (isTransitioning)
			return;

		StartCoroutine(TransitionWork(name));
	}
}