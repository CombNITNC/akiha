using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Measurer : MonoBehaviour {
	[SerializeField] GameObject door;
	[SerializeField] bool visibleTime = true;
	float time = 0.0f, startTime = 0.0f, highscore = 0.0f;
	bool measuringTime = false;

	Text currentTimeText;
	Text recordText;
	Text diffText;

	GameController gameController;

	AudioSource source;
	[SerializeField] AudioClip clearSound;
	[SerializeField] AudioClip highscoreSound;

	// Use this for initialization
	void Start() {
		source = gameObject.AddComponent<AudioSource>();
		source.clip = clearSound;
		startTime = Time.time;
		time = startTime;
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}

	public void Init(Text _current, Text _record, Text _diff) {
		currentTimeText = _current;
		recordText = _record;
		diffText = _diff;
	}

	public void MeasureStart(float _highscore = 10000.0f) {
		highscore = _highscore;
		recordText.text = highscore.ToString("00.0000");
		measuringTime = true;
	}

	// Update is called once per frame
	void FixedUpdate() {
		if (measuringTime) {
			time += Time.fixedDeltaTime;
			if (visibleTime) {
				currentTimeText.text = time.ToString("00.0000");
			}

			if (highscore == 10000.0f) {
				diffText.text = "First record.";
			}
			else if ((time - startTime) < highscore) {
				source.clip = highscoreSound;
				diffText.text = "-" + (highscore - time + startTime).ToString("00.000");
				diffText.color = Color.green;
			}
			else {
				diffText.text = "+" + (time - startTime - highscore).ToString("00.000");
				diffText.color = Color.red;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Player" && measuringTime) {
			measuringTime = false;
			if (visibleTime) {
				recordText.text = (time - startTime).ToString("00.0000");
			}

			gameController.Goal(time - startTime);
			source.Play();
		}
		door.SetActive(false);
	}

	void OnCollisionExit2D(Collision2D col) {
		door.SetActive(true);
	}
}