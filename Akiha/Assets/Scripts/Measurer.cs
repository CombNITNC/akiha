using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
[RequireComponent(typeof(Collider2D))]
public class Measurer : MonoBehaviour {
	[SerializeField] GameObject door;
	[SerializeField] bool visibleTime = true;
	float time = 0.0f, startTime = 0.0f, highscore = 0.0f;
	bool measuringTime = false;

	Text currentTimeText;
	Text diffText;

	public delegate void GoalDelegate(float time);
	GoalDelegate delegater;

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

		time += Time.deltaTime;
		if (visibleTime) {
			currentTimeText.text = (time - startTime).ToString("00.0000");
		}

		if (highscore == 10000.0f) {
			diffText.text = "First record.";
		} else {
			var diff = time - startTime - highscore;
			if ((time - startTime) < highscore) {
				source.clip = highscoreSound;
				diffText.text = "-" + (-diff).ToString("00.000");
				diffText.color = Color.green;
			} else {
				diffText.text = "+" + diff.ToString("00.000");
				diffText.color = Color.red;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		measuringTime = false;
		if (col.gameObject.tag == "Player") {
			if (delegater != null)
				delegater(time - startTime);
			source.Play();
		}
		door.SetActive(false);
	}

	void OnTriggerExit2D(Collider2D col) {
		GetComponent<Collider2D>().isTrigger = false;
		door.SetActive(true);
	}

	public static Measurer AttachMeasure(GameObject target, Text _current, Text _diff, GoalDelegate _delegater, float _highscore = 10000.0f) {
		var self = target.GetComponent<Measurer>();
		if (self == null)
			self = target.AddComponent<Measurer>();

		self.delegater = _delegater;
		self.currentTimeText = _current;
		self.diffText = _diff;
		self.highscore = _highscore;
		return self;
	}

	public void MeasureStart() {
		startTime = Time.time;
		time = startTime;
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