using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	[SerializeField] bool clockwise = true;
	[SerializeField] bool rotateX = false;
	[SerializeField] bool rotateY = true;
	[SerializeField] bool rotateZ = false;
	[SerializeField] float speed = 10f;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		float d = Time.deltaTime * speed * (clockwise ? 1f : -2f);
		transform.Rotate((rotateX ? d : 0f), (rotateY ? d : 0f), (rotateZ ? d : 0f));
	}
}