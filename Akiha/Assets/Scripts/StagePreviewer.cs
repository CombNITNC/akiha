using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
class PreviewContainer {
	public GameObject stage;
	public Camera camera;
	public Animation anim;
}

[RequireComponent(typeof(GameStorageManager), typeof(ScreenWiper))]
public class StagePreviewer : MonoBehaviour {
	[SerializeField] PreviewContainer[] previews = new PreviewContainer[5];
	[SerializeField] float cycleDuration = 5.0f;
	[SerializeField] float fadeTime = 0.5f;

	int viewingIndex = 0;
	float threshold = 0f;
	float start = 0f;

	[SerializeField] Text layerLabel;
	[SerializeField] Text scoreLabel;
	float[] tmpScores = new float[5];

	void Start() {
		thershold = Screen.width / 2.5f;
		GenerateAnimations();
		saver = GetComponent<GameStorageManager>();
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
					if (Math.abs(start - touch.position.x) > threshold) {
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
		foreach (PreviewContainer c in previews) {
			c.anim = AddComponent<Animation>();

			Vector3 stageEnd = Vector3.zero;
			foreach (Transform child in c.stage.transform) {
				if (child.tag == "Goal") {
					stageEnd = child.position;
				}
			}

			AnimationClip clip = new AnimationClip();
			AnimationCurve curveX = AnimationCurve.EaseInOut(0.0f, 0.0f, cycleDuration, stageEnd.x);
			AnimationCurve curveY = AnimationCurve.EaseInOut(0.0f, 0.0f, cycleDuration, stageEnd.y);
			clip.SetCurve("", typeof(Transform), "localPosition.x", curveX);
			clip.SetCurve("", typeof(Transform), "localPosition.y", curveY);

			c.anim.AddClip(clip, "Preview");
		}
	}

	void UpdateLabel() {
		layerLabel.text = "LAYER: " + (viewingIndex + 1).ToString();
		var highscore = tmpScores[viewingIndex];
		scoreLabel.text = "SCORE: " + (highscore == 10000.0f ? "NONE" : highscore.ToString("00.000"));
	}

	public void ViewNext() {
		if (viewingIndex < 4) {
			GetComponent<ScreenWiper>().CrossFadePro(previews[viewingIndex].camera, previews[viewingIndex + 1].camera, fadeTime);
			previews[viewingIndex + 1].anim.Play();
			++viewingIndex;
		}
		UpdateLabel();
	}

	public void ViewPrev() {
		if (0 < viewingIndex) {
			GetComponent<ScreenWiper>().CrossFadePro(previews[viewingIndex].camera, previews[viewingIndex - 1].camera, fadeTime);
			previews[viewingIndex - 1].anim.Play();
			--viewingIndex;
		}
		UpdateLabel();
	}
}