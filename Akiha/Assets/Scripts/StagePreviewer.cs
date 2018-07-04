using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
class PreviewContainer {
	public GameObject stage = null;
	public Camera camera = null;

	public bool IsValid() {
		return stage != null && camera != null;
	}
}

[RequireComponent(typeof(GameStorageManager), typeof(ScreenWiper))]
public class StagePreviewer : MonoBehaviour {
	Animation[] anims = new Animation[5];
	[SerializeField] PreviewContainer[] previews = new PreviewContainer[5];
	[SerializeField] float cycleDuration = 5.0f;
	[SerializeField] float fadeTime = 0.2f;

	int viewingIndex = 0;
	float threshold = 0f;
	float start = 0f;

	[SerializeField] Text layerLabel;
	[SerializeField] Text scoreLabel;
	float[] tmpScores = new float[5];

	void Start() {
		threshold = Screen.width / 2.5f;
		GenerateAnimations();
		if (anims[0] != null) {
			anims[0].Play("Preview");
		}

		var saver = GetComponent<GameStorageManager>();
		saver.Load(out tmpScores);
		UpdateLabel();
	}

	void Update() {
		if (0 < Input.touchCount) {
			var touch = Input.GetTouch(0);
			switch (touch.phase) {
				case TouchPhase.Began:
					start = touch.position.x;
					break;
				case TouchPhase.Ended:
					if (Mathf.Abs(start - touch.position.x) > threshold) {
						if (start > touch.position.x) {
							ViewPrev();
						}
						else {
							ViewNext();
						}
					}
					break;
			}
		}
	}

	void GenerateAnimations() {
		for (int i = 0; i < previews.Length; ++i) {
			if (!previews[i].IsValid())
				continue;

			var stage = Instantiate(previews[i].stage, new Vector3(i * 100f, 0f, 0f), Quaternion.identity);

			anims[i] = previews[i].camera.gameObject.AddComponent<Animation>();

			Vector3 stageEnd = Vector3.zero;
			foreach (Transform child in stage.transform) {
				if (child.tag == "End") {
					stageEnd = child.position;
				}
			}

			AnimationClip clip = new AnimationClip();
			clip.legacy = true;
			clip.wrapMode = WrapMode.Loop;
			AnimationCurve curveX = AnimationCurve.EaseInOut(0.0f, i * 100f, cycleDuration, stageEnd.x + i * 100f);
			AnimationCurve curveY = AnimationCurve.EaseInOut(0.0f, 0.0f, cycleDuration, stageEnd.y);
			AnimationCurve curveZ = AnimationCurve.EaseInOut(0.0f, -20f, cycleDuration, -20f);
			clip.SetCurve("", typeof(Transform), "m_LocalPosition.x", curveX);
			clip.SetCurve("", typeof(Transform), "m_LocalPosition.y", curveY);
			clip.SetCurve("", typeof(Transform), "m_LocalPosition.z", curveZ);

			anims[i].AddClip(clip, "Preview");
		}
	}

	void UpdateLabel() {
		layerLabel.text = "AREA: " + (viewingIndex + 1).ToString();
		var highscore = tmpScores[viewingIndex];
		scoreLabel.text = "SCORE: " + (highscore == 10000.0f ? "NONE" : highscore.ToString("00.000"));
	}

	public void ViewNext() {
		if (viewingIndex < 4) {
			if (!previews[viewingIndex + 1].IsValid()) {
				return;
			}

			GetComponent<ScreenWiper>().CrossFadePro(previews[viewingIndex].camera, previews[viewingIndex + 1].camera, fadeTime);
			anims[viewingIndex].Stop();
			anims[viewingIndex + 1].Play("Preview");
			++viewingIndex;
		}
		UpdateLabel();
	}

	public void ViewPrev() {
		if (0 < viewingIndex) {
			if (!previews[viewingIndex - 1].IsValid()) {
				return;
			}

			GetComponent<ScreenWiper>().CrossFadePro(previews[viewingIndex].camera, previews[viewingIndex - 1].camera, fadeTime);
			anims[viewingIndex].Stop();
			anims[viewingIndex - 1].Play("Preview");
			--viewingIndex;
		}
		UpdateLabel();
	}

	public void GoToMainMenu() {
		SceneManager.LoadScene("MainMenu");
	}
}