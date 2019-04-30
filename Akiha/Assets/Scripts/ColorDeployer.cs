using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColorDeployer : MonoBehaviour, IHasColor {
	[SerializeField] Color32 delpoyingColor = Color.red;
	AudioSource source;
	[SerializeField] AudioClip depolySound = null;

	void Start() {
		if (GetComponent<AudioSource>() == null)
			source = gameObject.AddComponent<AudioSource>();
		else
			source = GetComponent<AudioSource>();

		source.clip = depolySound;
		source.playOnAwake = false;
	}

	public Color32 GetColor() {
		source.Play();
		return delpoyingColor;
	}
}