using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ColorApplier), typeof(BoxCollider2D))]
public class ColorBarrier : MonoBehaviour, IHasColor {
	GameObject gameController;
	[SerializeField] Color32 weakColor;
	[SerializeField] GameObject particle;

	// Use this for initialization
	void Start() {
		gameController = GameObject.FindWithTag("GameController");
	}

	public Color32 GetColor() {
		return weakColor;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (gameController.GetComponent<GameController>().GetPlayer().GetColor().IsEqualRGB(weakColor)) {
			GetComponent<BoxCollider2D>().isTrigger = true;
			Instantiate(particle, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}
}