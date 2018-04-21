using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour {
	[SerializeField] Color activatableColor;
	GameObject gameController;
	[SerializeField] float jumpDuration = 1.5f;

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material.color = activatableColor;
		gameController = GameObject.FindWithTag("GameController");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		gameController.GetComponent<GameController>().GetPlayer().StartJump(jumpDuration);
	}
}
