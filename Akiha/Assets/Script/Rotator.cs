using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	[SerializeField] bool clockwise = true;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		transform.Rotate(0f, Time.deltaTime * 10f * (clockwise ? 1f : -2f), 0f);
	}
}