using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class ArrayDuplicatorParam {
	[SerializeField] public GameObject target = null;
	[SerializeField] public Vector3 count = new Vector3(1, 1, 1);
	[SerializeField] public Vector3 padding = new Vector3(1, 1, 1);
	[SerializeField] public Vector3 rotateOffset = Vector3.zero;
	[SerializeField] public Vector3 scaleOffset = Vector3.zero;
}

public class ArrayDuplicator : MonoBehaviour {
	[SerializeField] ArrayDuplicatorParam[] pars = new ArrayDuplicatorParam[1];

	void Start() {
		foreach (var param in pars) {
			GameObject prevObj = param.target;
			for (int z = 0; z < param.count.z; ++z) {
				for (int y = 0; y < param.count.y; ++y) {
					for (int x = 0; x < param.count.x; ++x) {
						var clone = Instantiate(prevObj, Vector3.zero, Quaternion.identity);
						clone.transform.parent = prevObj.transform;
						clone.transform.localScale = param.scaleOffset;
						clone.transform.localRotation = Quaternion.Euler(param.rotateOffset);
						clone.transform.localPosition = param.padding;

						prevObj = clone;
					}
				}
			}
		}
	}
}