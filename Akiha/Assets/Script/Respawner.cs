using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour {
	[SerializeField] GameObject gameController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		var respawnPos = transform.position;
		respawnPos.z = 0;
		gameController.GetComponent<GameController>().SetRespawn(respawnPos);
	}
}
