using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finisher : MonoBehaviour {
	GameController gameController;

	// Use this for initialization
	void Start() {
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}

	// Update is called once per frame
	void Update() {

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			gameController.Finish(transform.position);
		}
	}
}