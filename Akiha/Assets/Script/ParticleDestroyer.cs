using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour {
	ParticleSystem ps;
	AudioSource source;
	[SerializeField] AudioClip breakSound;

	// Use this for initialization
	void Start () {
		source = gameObject.AddComponent<AudioSource>();
		source.clip = breakSound;
		ps = GetComponent<ParticleSystem>();
		source.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (ps) {
			if (!ps.IsAlive()) {
				Destroy(gameObject);
			}
		}
	}
}
