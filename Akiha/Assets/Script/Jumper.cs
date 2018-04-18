using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour {
	[SerializeField] Color activatableColor;
	[SerializeField] GameObject gameController;
	[SerializeField] float jumpDuration = 1.5f;

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material.color = activatableColor;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		gameController.GetComponent<GameController>().GetPlayer().StartJump(jumpDuration);
	}
}
