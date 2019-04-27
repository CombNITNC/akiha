using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class Measurer : MonoBehaviour {
	[SerializeField] GameObject door;
	[SerializeField] bool visibleTime = true;
	float time = 0.0f, startTime = 0.0f, highscore = 0.0f;
	bool measuringTime = false;

	Text currentTimeText;
	Text recordText;
	Text diffText;

	public delegate void GoalDelegate(float time);
	GoalDelegate delegater;

	AudioSource source;
	[SerializeField] AudioClip clearSound;
	[SerializeField] AudioClip highscoreSound;

	// Use this for initialization
	void Start() {
		source = gameObject.AddComponent<AudioSource>();
		source.clip = clearSound;
	}

	void Update() {
		if (currentTimeText == null || diffText == null) return;
		if (measuringTime) {
			time += Time.deltaTime;
			if (visibleTime) {
				currentTimeText.text = (time - startTime).ToString("00.0000");
			}

			if (highscore == 10000.0f) {
				diffText.text = "First record.";
			} else if ((time - startTime) < highscore) {
				source.clip = highscoreSound;
				diffText.text = "-" + (-(time - startTime - highscore)).ToString("00.000");
				diffText.color = Color.green;
			} else {
				diffText.text = "+" + (time - startTime - highscore).ToString("00.000");
				diffText.color = Color.red;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		measuringTime = false;
		if (col.gameObject.tag == "Player") {
			if (visibleTime) {
				var text = (time - startTime).ToString("00.0000");
				recordText.text = text;
			}

			delegater(time - startTime);
			source.Play();
		}
		door.SetActive(false);
	}

	void OnTriggerExit2D(Collider2D col) {
		GetComponent<Collider2D>().isTrigger = false;
		door.SetActive(true);
	}

	public void Init(Text _current, Text _record, Text _diff, GoalDelegate _delegater) {
		delegater = _delegater;
		currentTimeText = _current;
		recordText = _record;
		diffText = _diff;
	}

	public void MeasureStart(float _highscore = 10000.0f) {
		highscore = _highscore;
		recordText.text = highscore.ToString("00.0000");
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