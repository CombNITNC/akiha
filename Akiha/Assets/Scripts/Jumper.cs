using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ColorApplier), typeof(CircleCollider2D), typeof(Animation))]
public class Jumper : MonoBehaviour, IHasColor, ICollideWithColor {
	[SerializeField] Color activatableColor = Color.red;
	[SerializeField] float jumpDuration = 3.0f;

	AudioSource source;
	[SerializeField] AudioClip jumpSound = null;

	Animation anim;
	PlayerController player;

	void Start() {
		if (GetComponent<AudioSource>() == null)
			source = gameObject.AddComponent<AudioSource>();
		else
			source = GetComponent<AudioSource>();

		source.clip = jumpSound;
		source.playOnAwake = false;

		anim = GetComponent<Animation>();
		player = GameObject.FindWithTag("GameController").GetComponent<GameController>().GetPlayer();
	}

	public Color32 GetColor() {
		return activatableColor;
	}

	public void CollideWith(Color32 color) {
		if (color.IsEqualRGB(activatableColor)) {
			player.StartJump(jumpDuration);
			source.Play();
			anim.Play();
		}
	}
}