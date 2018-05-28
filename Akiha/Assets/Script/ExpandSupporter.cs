using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandSupporter : MonoBehaviour {

	public void BringFront() {
		transform.SetAsLastSibling();
	}
}