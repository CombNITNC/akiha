using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class ColorDeployer : MonoBehaviour, IHasColor, ICollideWithColor {
	[SerializeField] Color32 delpoyingColor = Color.red;
	AudioSource source;
	[SerializeField] AudioClip depolySound = null;

	void Start() {
		source = GetComponent<AudioSource>();
		source.clip = depolySound;
		source.playOnAwake = false;
	}

	public Color32 GetColor() {
		return delpoyingColor;
	}

	public void CollideWith(Color32 color, PlayerController player) {
		source.Play();
	}
}