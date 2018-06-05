using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Renderer), typeof(BoxCollider2D))]
public class ColorBarrier : MonoBehaviour {
	GameObject gameController;
	[SerializeField] Color32 weakColor;
	[SerializeField] GameObject particle;

	// Use this for initialization
	void Start() {
		GetComponent<Renderer>().material.color = weakColor;
		gameController = GameObject.FindWithTag("GameController");
	}

	// Update is called once per frame
	void Update() {

	}

	void OnCollisionEnter2D(Collision2D other) {
		if (IsEuqalRGB(gameController.GetComponent<GameController>().GetPlayer().GetColor(), weakColor)) {
			GetComponent<BoxCollider2D>().isTrigger = true;
			Instantiate(particle, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}

	bool IsEuqalRGB(Color lhs, Color rhs) {
		var inR = (Math.Abs(lhs.r - rhs.r) < 0.008);
		var inG = (Math.Abs(lhs.g - rhs.g) < 0.008);
		var inB = (Math.Abs(lhs.b - rhs.b) < 0.008);
		return inR && inG && inB;
	}
}