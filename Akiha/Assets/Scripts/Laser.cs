using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Renderer), typeof(ColorApplier))]
public class Laser : MonoBehaviour, ICollideWithColor, IHasColor {
	[SerializeField] float emitInterval = 2.0f;
	[SerializeField] float emitDuration = 2.0f;
	[SerializeField] Color32 lazerColor = Color.white;
	Collider2D col;
	Renderer rend;
	float time = 0.0f;

	// Use this for initialization
	void Start() {
		col = GetComponent<Collider2D>();
		rend = GetComponent<Renderer>();
	}

	// Update is called once per frame
	void Update() {
		time += Time.deltaTime;

		if (emitInterval == 0.0f) { return; }
		// Transition the state
		if (col.enabled) {
			if (time > emitDuration) {
				time = 0f;
				col.enabled = false;
				rend.enabled = false;
			}
		} else {
			if (time > emitInterval) {
				time = 0f;
				col.enabled = true;
				rend.enabled = true;
			}
		}
	}

	public Color32 GetColor() {
		return lazerColor;
	}

	public void CollideWith(Color32 color, PlayerController player) {
		if (!lazerColor.IsEqualRGB(color))
			player.Crush();
	}
}