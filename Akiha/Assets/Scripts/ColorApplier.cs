using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ColorApplier : MonoBehaviour {
	Material coloredMat;

	void OnDestroy() {
		DestroyImmediate(coloredMat);
	}

	public void Apply(Color c) {
		var rend = GetComponent<Renderer>();
		if (rend != null) {
			for (int i = 0; i < rend.sharedMaterials.Length; ++i) {
				var mat = rend.sharedMaterials[i];
				if (mat.IsKeywordEnabled("_ALPHAPREMULTIPLY_ON")) {
					coloredMat = new Material(mat);
					coloredMat.color = c;
					mat = coloredMat;
				}
			}
		}
	}
}