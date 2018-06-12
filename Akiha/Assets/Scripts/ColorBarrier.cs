using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(ColorApplier), typeof(BoxCollider2D))]
public class ColorBarrier : MonoBehaviour {
	GameObject gameController;
	[SerializeField] Color32 weakColor;
	[SerializeField] GameObject particle;

	// Use this for initialization
	void Start() {
		gameController = GameObject.FindWithTag("GameController");
	}

	// Update is called once per frame
	void Update() {

	}

	void OnValidate() {
		GetComponent<ColorApplier>().Apply(weakColor);
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (gameController.GetComponent<GameController>().GetPlayer().GetColor().IsEqualRGB(weakColor)) {
			GetComponent<BoxCollider2D>().isTrigger = true;
			Instantiate(particle, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}
}