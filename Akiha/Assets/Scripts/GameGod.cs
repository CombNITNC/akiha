using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameGod : MonoBehaviour {
	static bool made = false;
	GameStory story;

	void Awake() {
		if (!made) {
			made = true;
			DontDestroyOnLoad(gameObject);
			var gods = GameObject.FindGameObjectsWithTag("God");
			if (2 <= gods.Length) {
				for (int i = 0; i < gods.Length - 1; ++i) {
					if (gods[i] != this) {
						Destroy(gods[i]);
					}
				}
			}

			CheckStory();
		}
	}

	void CheckStory() {
		story = GetComponent<GameStory>();
		if (story == null) {
			story = gameObject.AddComponent<GameStory>();
		}
	}

	public GameStory GetStory() {
		return story;
	}

	IEnumerator ExecuteStoryWork() {
		yield return SceneManager.LoadSceneAsync("Game");
	}

	void ExecuteStory() {
		if (story.Invalid())
			return;

		StartCoroutine(ExecuteStoryWork());
	}

	public void ExecuteAllStory() {
		CheckStory();
		story.All();
		ExecuteStory();
	}

	public void ExecuteSingleStory(int chapter) {
		CheckStory();
		story.Single(chapter);
		ExecuteStory();
	}
}