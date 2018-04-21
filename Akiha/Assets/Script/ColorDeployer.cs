using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColorDeployer : MonoBehaviour {
	[SerializeField] Color32 delpoyingColor = Color.red;
	[SerializeField] GameObject gameController;
	Renderer rend;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
		rend.material.color = delpoyingColor;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D other) {
		gameController.GetComponent<GameController>().GetPlayer().SetColor(delpoyingColor);
	}
}
