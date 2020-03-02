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
			bool needToScale = param.scaleOffset != Vector3.zero;
			for (int z = 0; z < param.count.z; ++z) {
				for (int y = 0; y < param.count.y; ++y) {
					for (int x = 0; x < param.count.x; ++x) {
						var clone = Instantiate(param.target, Vector3.zero, Quaternion.identity);
						clone.transform.parent = transform;
						if (needToScale) {
							clone.transform.localScale = Vector3.Scale(param.scaleOffset, new Vector3(x, y, z));
						}
						clone.transform.localRotation = Quaternion.Euler(param.rotateOffset);
						clone.transform.localPosition = Vector3.Scale(param.padding, new Vector3(x, y, z));
					}
				}
			}
		}
	}
}