using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	PlayerController player;

	[SerializeField] GameObject[] stages;
	float stagesPointEnd = 0.0f;
	int loadedIndex = 0;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		LoadStage();
		LoadStage(); // Prefetching
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void LoadStage() {
		var newStage = Instantiate(stages[loadedIndex], new Vector3(0, stagesPointEnd, 0), Quaternion.identity);
		Bounds bounds = new Bounds();
		GetTotalBounds(bounds, newStage.transform);
		stagesPointEnd += bounds.size.y;
		loadedIndex++;
	}

	void GetTotalBounds(Bounds bounds, Transform target) {
		foreach(Transform child in target) {
			if (child.GetComponent<Renderer>()) {
				bounds.Encapsulate(child.GetComponent<Renderer>().bounds.min);
				bounds.Encapsulate(child.GetComponent<Renderer>().bounds.max);
			}
			GetTotalBounds(bounds, child);
		}
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
