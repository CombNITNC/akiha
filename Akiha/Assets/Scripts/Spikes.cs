﻿using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Spikes : MonoBehaviour, ICollideWithColor {
	[SerializeField] bool rotateClockwise = false;
	[SerializeField] float rotateSpeed = 40.0f;

	// Update is called once per frame
	void Update() {
		var deg = Time.deltaTime * rotateSpeed * (rotateClockwise ? -1.0f : 1.0f);
		transform.Rotate(0f, 0f, deg);
	}

	public void CollideWith(CMYK color, PlayerController player) {
		player.Crush();
	}
}