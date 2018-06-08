﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour {
	GameObject gameController;
	AudioSource source;
	[SerializeField] AudioClip registerSound;

	// Use this for initialization
	void Start () {
		source = gameObject.AddComponent<AudioSource>();
		source.clip = registerSound;
		gameController = GameObject.FindWithTag("GameController");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		var respawnPos = transform.position;
		respawnPos.z = 0;
		gameController.GetComponent<GameController>().SetRespawn(respawnPos);
		GetComponent<Renderer>().material.color = Color.cyan;
		source.Play();
	}
}