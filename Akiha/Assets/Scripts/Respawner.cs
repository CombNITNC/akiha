using UnityEngine;

public class Respawner : MonoBehaviour, ICollideWithColor {
	AudioSource source;
	[SerializeField] AudioClip registerSound = null;

	// Use this for initialization
	void Start() {
		source = gameObject.AddComponent<AudioSource>();
		source.clip = registerSound;
	}

	public void CollideWith(CMYK color, PlayerController player) {
		var respawnPos = transform.position;
		respawnPos.z = 0;
		player.SetRespawn(respawnPos);
		GetComponent<Renderer>().material.color = Color.cyan;
		source.Play();
	}
}