using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Measurer : MonoBehaviour {
	[SerializeField] GameObject door;
	[SerializeField] bool visibleTime = true;
	float time = 0.0f, startTime = 0.0f;
	bool measuringTime = true;

	[SerializeField] Text currentTimeText;
	[SerializeField] Text recordText;

	GameController gameController;

	AudioSource source;
	[SerializeField] AudioClip clearSound;
	[SerializeField] AudioClip highscoreSound;

	// Use this for initialization
	void Start () {
		source = gameObject.AddComponent<AudioSource>();
		source.clip = highscoreSound;
		startTime = Time.time;
		time = startTime;
		currentTimeText = GameObject.Find("Timer").GetComponent<Text>();
		recordText = GameObject.Find("Record").GetComponent<Text>();
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (measuringTime) {
			time += Time.deltaTime;
		}
		if (visibleTime) {
			currentTimeText.text = time.ToString("00.0000");
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			measuringTime = false;
			if (visibleTime) {
				recordText.text = (time - startTime).ToString("00.0000");
			}

			gameController.LoadStage();
			source.Play();
		}
		door.SetActive(false);
	}

	void OnTriggerExit2D(Collider2D other) {
		door.SetActive(true);
	}
}
