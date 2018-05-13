using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	PlayerController player;

	[SerializeField] GameObject[] stages;
	Transform lastStageEnd = new Transform();
	int loadedIndex = 0;

	// Use this for initialization
	void Start() {
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		LoadStage();
		LoadStage(); // Prefetching
	}

	// Update is called once per frame
	void Update() { }

	public void LoadStage() {
		if (stages[loadedIndex] == null)
			return;

		var newStage = Instantiate(stages[loadedIndex], lastStageEnd, 0,
			Quaternion.identity);
		foreach (var child in newStage.transform.Find("")) {
			if (child.tag == "Goal")
				lastStageEnd = child;
		}
		loadedIndex++;
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