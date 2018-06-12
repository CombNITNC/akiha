using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
[RequireComponent(typeof(ColorApplier), typeof(Collider2D))]
public class ColorDeployer : MonoBehaviour {
	[SerializeField] Color32 delpoyingColor = Color.red;
	GameController gameController;

	AudioSource source;
	[SerializeField] AudioClip depolySound;

	// Use this for initialization
	void Start() {
		if (GetComponent<AudioSource>() == null)
			source = gameObject.AddComponent<AudioSource>();
		else
			source = GetComponent<AudioSource>();

		source.clip = depolySound;
		source.playOnAwake = false;
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}

	// Update is called once per frame
	void Update() {

	}

	void OnValidate() {
		GetComponent<ColorApplier>().Apply(delpoyingColor);
	}

	void OnTriggerEnter2D(Collider2D other) {
		gameController.GetPlayer().SetColor(delpoyingColor);
		source.Play();
	}
}