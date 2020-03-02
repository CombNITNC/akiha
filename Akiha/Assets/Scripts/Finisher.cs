using UnityEngine;

public class Finisher : MonoBehaviour {
	GameController gameController;

	// Use this for initialization
	void Start() {
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			gameController.Finish(transform.position);
		}
	}
}