using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Pauser : MonoBehaviour {
	Animator anim;
	bool isPausing = false;

	GameController gameController;

	void Awake() {
		anim = GetComponent<Animator>();
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}

	public void Pause() {
		isPausing = !isPausing;
		gameController.ToggleMeasurer();
		anim.SetBool("isPaused", isPausing);
	}
}