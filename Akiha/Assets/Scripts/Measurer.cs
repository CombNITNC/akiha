using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class Measurer : MonoBehaviour {
	[SerializeField] GameObject door = null;
	float elapsed = 0.0f;
	bool measuringTime = false;

	public delegate void ElapsedDelegate(float elapsed);
	ElapsedDelegate NotifyElapsed;

	public delegate bool GoalDelegate(float time);
	GoalDelegate OnGoal;

	AudioSource source;
	[SerializeField] AudioClip clearSound = null;
	[SerializeField] AudioClip highscoreSound = null;

	// Use this for initialization
	void Start() {
		source = gameObject.AddComponent<AudioSource>();
		source.clip = clearSound;
	}

	void Update() {
		if (!measuringTime) {
			return;
		}

		elapsed += Time.deltaTime;
		NotifyElapsed(elapsed);
	}

	void OnTriggerEnter2D(Collider2D col) {
		measuringTime = false;
		if (col.gameObject.tag == "Player") {
			if (OnGoal(elapsed)) {
				source.clip = highscoreSound;
			}
			source.Play();
		}
		if (door != null)
			door.SetActive(false);
	}

	void OnTriggerExit2D(Collider2D col) {
		GetComponent<Collider2D>().isTrigger = false;
		if (door != null)
			door.SetActive(true);
	}

	public static Measurer AttachMeasure(GameObject target, ElapsedDelegate _elapsed, GoalDelegate _goal) {
		if (_elapsed == null) {
			throw new ArgumentNullException("_elapsed");
		}
		if (_goal == null) {
			throw new ArgumentNullException("_goal");
		}

		var self = target.GetComponent<Measurer>();
		if (self == null)
			self = target.AddComponent<Measurer>();

		self.OnGoal = _goal;
		self.NotifyElapsed = _elapsed;
		return self;
	}

	public void MeasureStart() {
		elapsed = 0;
		measuringTime = true;
	}

	public void MeasureStop() {
		measuringTime = false;
	}

	public void MeasureResume() {
		measuringTime = true;
	}

	public bool IsMeasuring() { return measuringTime; }
}