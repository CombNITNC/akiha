using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Jumper : MonoBehaviour {
	[SerializeField] Color activatableColor;
	GameObject gameController;
	[SerializeField] float jumpDuration = 1.5f;

	AudioSource source;
	[SerializeField] AudioClip jumpSound;

	// Use this for initialization
	void Start() {
		source = gameObject.AddComponent<AudioSource>();
		source.clip = jumpSound;
		GetComponent<Renderer>().material.color = activatableColor;
		gameController = GameObject.FindWithTag("GameController");
	}

	// Update is called once per frame
	void Update() {

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (IsEuqalRGB(gameController.GetComponent<GameController>().GetPlayer().GetColor(), activatableColor)) {
			gameController.GetComponent<GameController>().GetPlayer().StartJump(jumpDuration);
			source.Play();
		}
	}

	bool IsEuqalRGB(Color lhs, Color rhs) {
		var inR = (Math.Abs(lhs.r - rhs.r) < 0.008);
		var inG = (Math.Abs(lhs.g - rhs.g) < 0.008);
		var inB = (Math.Abs(lhs.b - rhs.b) < 0.008);
		return inR && inG && inB;
	}
}