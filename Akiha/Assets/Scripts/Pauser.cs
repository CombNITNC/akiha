using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Pauser : MonoBehaviour {
	Animator anim;
	bool isPausing = false;

	GameController gameController;

	// Use this for initialization
	void Start() {
		anim = GetComponent<Animator>();
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}

	// Update is called once per frame
	void Update() {

	}

	public void Pause() {
		isPausing = !isPausing;
		gameController.ToggleMeasurer();
		anim.SetBool("isPaused", isPausing);
	}
}