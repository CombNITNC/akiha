using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBarrier : MonoBehaviour {
	[SerializeField] GameObject gameController;
	[SerializeField] Color weakColor;
	[SerializeField] GameObject particle;

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material.color = weakColor;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D (Collision2D other) {
		if (gameController.GetComponent<GameController>().GetPlayer().GetColor() == weakColor) {
			GetComponent<BoxCollider2D>().isTrigger = true;
			Instantiate(particle, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}
}
