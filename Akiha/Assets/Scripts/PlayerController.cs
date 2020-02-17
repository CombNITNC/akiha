using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public interface IHasColor {
	CMYK GetColor();
}

public interface ICollideWithColor {
	void CollideWith(CMYK color, PlayerController player);
}

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
[RequireComponent(typeof(CircleCollider2D), typeof(PlayerMover))]
[RequireComponent(typeof(PlayerPaintable))]
public class PlayerController : MonoBehaviour {
	Rigidbody2D body;
	Renderer rend;
	Animator anim;
	CircleCollider2D col;
	[SerializeField] GameObject playerObject = null;

	[SerializeField] float jumpHeight = 2.0f;
	bool isJumping = false;
	bool isFalling = false;
	bool isDead = false;
	Vector3 respawnPos;

	[SerializeField] GameObject crushParticle = null;
	[SerializeField] float crushWaitDuration = 1.5f;
	AudioSource source;
	[SerializeField] AudioClip fallSound = null;

	PlayerMover mover;
	PlayerPaintable paintable;

	// Use this for initialization
	void Awake() {
		body = GetComponent<Rigidbody2D>();
		rend = playerObject.GetComponent<Renderer>();
		anim = GetComponent<Animator>();
		respawnPos = transform.position;
		col = GetComponent<CircleCollider2D>();
		source = gameObject.AddComponent<AudioSource>();
		mover = GetComponent<PlayerMover>();
		mover.SetControlMode((ControlMode) PlayerPrefs.GetInt("ControlMode", 0));
		paintable = GetComponent<PlayerPaintable>();
		paintable.SetDelegate(ApplyColorDelegate);
		source.clip = fallSound;
	}

	// Update is called once per frame
	void Update() {
		if (IsOnFloor()) {
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
		col.enabled = false;
		var jumpingTime = 0f;

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

	void OnTriggerEnter2D(Collider2D col) {
		var color = col.GetComponent<IHasColor>();
		if (color != null && IsOnFloor()) {
			paintable.SetColor(color);
		}

		var collider = col.GetComponent<ICollideWithColor>();
		if (collider != null && IsOnFloor()) {
			collider.CollideWith(paintable.GetColor(), this);
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		var breakable = col.gameObject.GetComponent<ICollideWithColor>();
		if (breakable != null && IsOnFloor()) {
			breakable.CollideWith(paintable.GetColor(), this);
		}
	}

	void ApplyColorDelegate(CMYK color) {
		StartCoroutine(ApplyColorWithLerp(color.ToColor(), 0.1f));
	}

	IEnumerator ApplyColorWithLerp(Color dst, float duration) {
		var src = rend.material.color;
		for (float time = 0f; time <= duration; time += Time.deltaTime) {
			rend.material.color = Color32.Lerp(src, dst, time / duration);
			yield return null;
		}

		rend.material.color = dst;
		yield break;
	}

	bool IsOnFloor() {
		return !(isJumping || isDead || isFalling);
	}

	public void StartJump(float duration) {
		StartCoroutine(JumpWork(duration));
	}

	public void SetRespawn(Vector3 pos) {
		if (!IsOnFloor())
			return;

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
		if (!IsOnFloor())
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