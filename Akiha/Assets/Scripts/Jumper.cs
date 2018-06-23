using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ColorApplier), typeof(CircleCollider2D), typeof(Animation))]
public class Jumper : MonoBehaviour, IHasColor {
	[SerializeField] Color activatableColor;
	GameController gameController;
	[SerializeField] float jumpDuration = 3.0f;

	AudioSource source;
	[SerializeField] AudioClip jumpSound;

	Animation anim;

	void Start() {
		if (GetComponent<AudioSource>() == null)
			source = gameObject.AddComponent<AudioSource>();
		else
			source = GetComponent<AudioSource>();

		source.clip = jumpSound;
		source.playOnAwake = false;

		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		anim = GetComponent<Animation>();
	}

	public Color32 GetColor() {
		return activatableColor;
	}

	void OnTriggerEnter2D(Collider2D other) {
		var player = gameController.GetPlayer();
		if (player != null) {
			if (player.GetColor().IsEqualRGB(activatableColor)) {
				player.StartJump(jumpDuration);
				source.Play();
			}
		}

		anim.Play();
	}
}