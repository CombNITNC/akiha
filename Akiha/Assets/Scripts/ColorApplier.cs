using UnityEngine;

[RequireComponent(typeof(IHasColor))]
public class ColorApplier : MonoBehaviour {
	[SerializeField] Renderer rend = null;
	Color src;

	void Start() {
		src = GetComponent<IHasColor>().GetColor().ToColor();
		if (rend != null) {
			rend.materials[rend.materials.Length - 1].SetColor("_TintColor", src);
			rend.materials[rend.materials.Length - 1].color = src;
		}
	}

	void OnDestory() {
		for (int i = 0; i < rend.materials.Length; ++i) {
			DestroyImmediate(rend.materials[i]);
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = src;
		Gizmos.DrawWireCube(transform.position, Vector3.one);
	}
}