using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ColorApplier), typeof(CircleCollider2D), typeof(Animation))]
public class Jumper : MonoBehaviour, IHasColor, ICollideWithColor {
	[SerializeField] CMYK activatableColor = CMYK.Red;
	[SerializeField] float jumpDuration = 3.0f;

	AudioSource source;
	[SerializeField] AudioClip jumpSound = null;

	Animation anim;

	void Start() {
		if (GetComponent<AudioSource>() == null)
			source = gameObject.AddComponent<AudioSource>();
		else
			source = GetComponent<AudioSource>();

		source.clip = jumpSound;
		source.playOnAwake = false;

		anim = GetComponent<Animation>();
	}

	public CMYK GetColor() {
		return activatableColor;
	}

	public void CollideWith(CMYK color, PlayerController player) {
		if (color == activatableColor) {
			player.StartJump(jumpDuration);
			source.Play();
			anim.Play();
		}
	}
}