using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ColorApplier), typeof(Collider2D))]
public class ColorDeployer : MonoBehaviour, IHasColor {
	[SerializeField] Color32 delpoyingColor = Color.red;
	GameController gameController;

	AudioSource source;
	[SerializeField] AudioClip depolySound;

	void Start() {
		if (GetComponent<AudioSource>() == null)
			source = gameObject.AddComponent<AudioSource>();
		else
			source = GetComponent<AudioSource>();

		source.clip = depolySound;
		source.playOnAwake = false;

		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}

	public Color32 GetColor() {
		return delpoyingColor;
	}

	void OnTriggerEnter2D(Collider2D other) {
		gameController.GetPlayer().SetColor(delpoyingColor);
		source.Play();
	}
}