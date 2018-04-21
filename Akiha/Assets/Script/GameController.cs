using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	PlayerController player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Respawn() {
		player.Respawn();
	}

	public PlayerController GetPlayer() {
		return player;
	}

	public void SetRespawn(Vector3 pos) {
		player.SetRespawn(pos);
	}
}
