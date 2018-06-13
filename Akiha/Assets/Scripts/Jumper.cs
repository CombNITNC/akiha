using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ColorApplier))]
public class Jumper : MonoBehaviour, IHasColor {
	[SerializeField] Color activatableColor;
	GameObject gameController;
	[SerializeField] float jumpDuration = 1.5f;

	AudioSource source;
	[SerializeField] AudioClip jumpSound;

	void Start() {
		if (GetComponent<AudioSource>() == null)
			source = gameObject.AddComponent<AudioSource>();
		else
			source = GetComponent<AudioSource>();

		source.clip = jumpSound;
		source.playOnAwake = false;

		gameController = GameObject.FindWithTag("GameController");
	}

	public Color32 GetColor() {
		return activatableColor;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (gameController.GetComponent<GameController>().GetPlayer().GetColor().IsEqualRGB(activatableColor)) {
			gameController.GetComponent<GameController>().GetPlayer().StartJump(jumpDuration);
			source.Play();
		}
	}
}