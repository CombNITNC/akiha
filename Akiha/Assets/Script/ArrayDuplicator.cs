using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayDuplicator : MonoBehaviour {
	[SerializeField] Vector3 count = Vector3.one;
	[SerializeField] Vector3 padding;
	[SerializeField] Vector3 rotateOffset;

	void Start() {
		for (int z = 0; z < count.z; ++z) {
			for (int y = 0; y < count.y; ++y) {
				for (int x = 0; x < count.x; ++x) {
					var clone = Instantiate(gameObject, new Vector3(x * padding.x + transform.position.x, y * padding.y + transform.position.y, z * padding.z + transform.position.z), Quaternion.identity);
					Destroy(clone.GetComponent<ArrayDuplicator>());
					clone.transform.parent = transform.parent;
					clone.transform.Rotate(x * rotateOffset);
				}
			}
		}
	}
}