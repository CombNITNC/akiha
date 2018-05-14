using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class ColorDeployer : MonoBehaviour {
	[SerializeField] Color32 delpoyingColor = Color.red;
	GameObject gameController;
	Renderer rend;

	AudioSource source;
	[SerializeField] AudioClip depolySound;

	// Use this for initialization
	void Start() {
		source = gameObject.AddComponent<AudioSource>();
		source.clip = depolySound;
		rend = GetComponent<Renderer>();
		rend.material.color = delpoyingColor;
		gameController = GameObject.FindWithTag("GameController");
	}

	// Update is called once per frame
	void Update() {

	}

	void OnTriggerEnter2D(Collider2D other) {
		gameController.GetComponent<GameController>().GetPlayer().SetColor(delpoyingColor);
		source.Play();
	}
}