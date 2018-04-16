using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColorDeployer : MonoBehaviour {
	[System.Serializable] public class ColorDeployEmitter : UnityEvent<Color> { }

	[SerializeField] Color delpoying_color = Color.red;
	[SerializeField] ColorDeployEmitter emitter = new ColorDeployEmitter();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D (Collision2D other) {
		emitter.Invoke(delpoying_color);
	}
}
