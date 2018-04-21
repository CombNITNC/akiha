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

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		time = startTime;
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
		}
		door.SetActive(false);
	}

	void OnTriggerExit2D(Collider2D other) {
		door.SetActive(true);
	}
}
