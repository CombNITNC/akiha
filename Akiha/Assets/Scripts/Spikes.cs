using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Spikes : MonoBehaviour {
	[SerializeField] bool rotateClockwise = false;
	[SerializeField] float rotateSpeed = 40.0f;

	GameController gameController;

	// Use this for initialization
	void Start() {
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}

	// Update is called once per frame
	void Update() {
		var deg = Time.deltaTime * rotateSpeed * (rotateClockwise ? -1.0f : 1.0f);
		transform.Rotate(0f, 0f, deg);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player")
			gameController.GetPlayer().Crush();
	}
}