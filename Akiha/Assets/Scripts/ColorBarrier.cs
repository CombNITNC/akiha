using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ColorApplier), typeof(BoxCollider2D))]
public class ColorBarrier : MonoBehaviour, IHasColor, ICollideWithColor {
	[SerializeField] CMYK weakColor = CMYK.Red;
	[SerializeField] GameObject particle = null;

	public CMYK GetColor() {
		return weakColor;
	}

	public void CollideWith(CMYK color, PlayerController player) {
		if (color == weakColor) {
			GetComponent<BoxCollider2D>().isTrigger = true;
			Instantiate(particle, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}
}