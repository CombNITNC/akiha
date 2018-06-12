using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(ColorApplier))]
public class Jumper : MonoBehaviour {
	[SerializeField] Color activatableColor;
	GameObject gameController;
	[SerializeField] float jumpDuration = 1.5f;

	AudioSource source;
	[SerializeField] AudioClip jumpSound;

	// Use this for initialization
	void Start() {
		if (GetComponent<AudioSource>() == null)
			source = gameObject.AddComponent<AudioSource>();
		else
			source = GetComponent<AudioSource>();

		source.clip = jumpSound;
		source.playOnAwake = false;
		gameController = GameObject.FindWithTag("GameController");
	}

	// Update is called once per frame
	void Update() {

	}

	void OnValidate() {
		GetComponent<ColorApplier>().Apply(activatableColor);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (gameController.GetComponent<GameController>().GetPlayer().GetColor().IsEqualRGB(activatableColor)) {
			gameController.GetComponent<GameController>().GetPlayer().StartJump(jumpDuration);
			source.Play();
		}
	}
}