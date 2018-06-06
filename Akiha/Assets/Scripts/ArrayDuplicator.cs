using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct ArrayDuplicatorParam {
	[SerializeField] public GameObject target;
	[SerializeField] public Vector3 count;
	[SerializeField] public Vector3 padding;
	[SerializeField] public Vector3 rotateOffset;
	[SerializeField] public Vector3 scaleOffset;
}

public class ArrayDuplicator : MonoBehaviour {
	[SerializeField] ArrayDuplicatorParam[] pars;

	void Start() {
		foreach (var param in pars) {
			GameObject prevObj = param.target;
			for (int z = 0; z < param.count.z; ++z) {
				for (int y = 0; y < param.count.y; ++y) {
					for (int x = 0; x < param.count.x; ++x) {
						var posX = x * param.padding.x;
						var posY = y * param.padding.y;
						var posZ = z * param.padding.z;
						var clone = Instantiate(prevObj, Vector3.zero, Quaternion.identity);
						clone.transform.parent = prevObj.transform;
						clone.transform.localScale = param.scaleOffset;
						clone.transform.rotation = Quaternion.Euler(param.rotateOffset);
						clone.transform.localPosition = new Vector3(posX, posY, posZ);

						prevObj = clone;
					}
				}
			}
		}
	}
}