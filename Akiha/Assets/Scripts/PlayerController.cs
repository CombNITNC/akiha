using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
[RequireComponent(typeof(CircleCollider2D), typeof(PlayerMover))]
public class PlayerController : MonoBehaviour {
	Rigidbody2D body;
	Renderer rend;
	Animator anim;
	CircleCollider2D col;
	[SerializeField] GameObject playerObject;

	[SerializeField] Color32 color = Color.white;
	[SerializeField] float colorSetWaitDuration = 0.2f;
	float colorSetCounter = 0.2f;

	[SerializeField] float jumpHeight = 2.0f;
	bool isJumping = false;
	bool isFalling = false;
	bool isDead = false;
	Vector3 respawnPos;

	[SerializeField] GameObject crushParticle;
	[SerializeField] float crushWaitDuration = 1.5f;

	AudioSource source;
	[SerializeField] AudioClip fallSound;

	PlayerMover mover;

	// Use this for initialization
	void Start() {
		body = GetComponent<Rigidbody2D>();
		rend = playerObject.GetComponent<Renderer>();
		rend.material.color = color;
		anim = GetComponent<Animator>();
		respawnPos = transform.position;
		col = GetComponent<CircleCollider2D>();
		source = gameObject.AddComponent<AudioSource>();
		mover = GetComponent<PlayerMover>();
		mover.SetControlMode((ControlMode) PlayerPrefs.GetInt("ControlMode", 0));
		source.clip = fallSound;
	}

	// Update is called once per frame
	void Update() {
		colorSetCounter += Time.deltaTime;

		if (!(isJumping || isFalling || isDead)) {
			DetectFalling();
		}
	}

	void DetectFalling() {
		var rayPos = transform.position;
		rayPos.z = -4;
		var ray = new Ray(rayPos, transform.forward);
		if (!Physics.SphereCast(ray, 0.5f, 10.0f)) {
			isFalling = true;
			anim.SetTrigger("fall");
			source.Play();
		}
	}

	IEnumerator JumpWork(float duration) {
		isJumping = true;
		var jumpingTime = 0f;
		col.enabled = false;

		Vector3 newPos = Vector3.zero;
		var a = jumpHeight / (duration * duration / 4);
		var t = 0f;
		do {
			jumpingTime += Time.deltaTime;
			newPos = transform.position;
			t = jumpingTime - duration / 2;

			newPos.z = -a * t * t + jumpHeight;
			transform.position = newPos;
			yield return null;
		} while (jumpingTime < duration);

		var posForReset = transform.position;
		posForReset.z = 0;
		transform.position = posForReset;
		col.enabled = true;
		isJumping = false;
		yield break;
	}

	public void SetColor(Color32 new_c) {
		if (colorSetCounter < colorSetWaitDuration || isDead || isFalling) {
			return;
		}

		colorSetCounter = 0.0f;

		if (new_c.IsEqualRGB(Color.black)) {
			color = Color.white;
		} else if (color.IsEqualRGB(Color.white)) {
			color = new_c;
		} else {
			color = Color32.Lerp(color, new_c, 0.5f);
		}
		ApplyColor();
	}

	public Color GetColor() {
		if (isFalling) {
			return Color.black;
		}
		return color;
	}

	void ApplyColor() {
		rend.material.color = color;
	}

	public void StartJump(float duration) {
		StartCoroutine(JumpWork(duration));
	}

	public void SetRespawn(Vector3 pos) {
		if (isFalling) {
			return;
		}
		respawnPos = pos;
	}

	public void Respawn() {
		transform.position = respawnPos;
		isFalling = false;
		body.velocity = Vector2.zero;
		rend.enabled = true;
		mover.enabled = true;
		col.enabled = true;
		isDead = false;
	}

	public void Crush() {
		if (isDead || isFalling)
			return;

		Instantiate(crushParticle, transform.position, transform.rotation);
		rend.enabled = false;
		col.enabled = false;
		mover.enabled = false;
		isDead = true;
		Invoke("Respawn", crushWaitDuration);
	}

	public void SetControlMode(ControlMode mode) {
		mover.SetControlMode(mode);
	}

	public void Finish(Vector3 finishPos) {
		mover.Stop(finishPos);
		mover.enabled = false;
		anim.SetTrigger("finish");
	}
}