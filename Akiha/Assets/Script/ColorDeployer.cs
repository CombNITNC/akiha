using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColorDeployer : MonoBehaviour {
	[SerializeField] Color delpoyingColor = Color.red;
	[SerializeField] GameObject gameController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D (Collision2D other) {
		gameController.GetComponent<GameController>().GetPlayer().SetColor(delpoyingColor);
	}
}
