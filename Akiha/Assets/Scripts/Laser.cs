using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Renderer))]
public class Laser : MonoBehaviour {
	[SerializeField] float emitInterval = 2.0f;
	[SerializeField] float emitDuration = 2.0f;
	Collider2D col;
	Renderer rend;
	float time = 0.0f;
	GameController gameController;

	// Use this for initialization
	void Start () {
		col = GetComponent<Collider2D>();
		rend = GetComponent<Renderer>();
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;

		// Transition the state
		if (col.enabled) {
			if (time > emitDuration) {
				time = 0f;
				col.enabled = false;
				rend.enabled = false;
			}
		} else {
			if (time > emitInterval) {
				time = 0f;
				col.enabled = true;
				rend.enabled = true;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (col.enabled) {
			gameController.GetPlayer().Crush();
		}
	}
}
