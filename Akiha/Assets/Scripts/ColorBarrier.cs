using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ColorApplier), typeof(BoxCollider2D))]
public class ColorBarrier : MonoBehaviour, IHasColor, ICollideWithColor {
	[SerializeField] Color32 weakColor = Color.red;
	[SerializeField] GameObject particle = null;

	public Color32 GetColor() {
		return weakColor;
	}

	public void CollideWith(Color32 color, PlayerController player) {
		if (color.IsEqualRGB(weakColor)) {
			GetComponent<BoxCollider2D>().isTrigger = true;
			Instantiate(particle, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}
}