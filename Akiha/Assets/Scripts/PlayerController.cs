using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(CircleCollider2D))]
public class PlayerController : MonoBehaviour {
	Rigidbody2D body;
	Renderer rend;
	Animator anim;
	CircleCollider2D col;
	[SerializeField] GameObject playerObject;

	[SerializeField] float maxLength;

	[SerializeField] Color32 color = Color.white;
	[SerializeField] float colorSetWaitDuration = 0.2f;
	float colorSetCounter = 0.2f;

	bool isJumping = false;
	float jumpingTime = 0.0f;
	float jumpingDuration = 0.0f;
	[SerializeField] float jumpHeight = 2.0f;

	bool isFalling = false;
	Vector3 respawnPos;
	bool canInput = true;
	bool isDead = false;
	[SerializeField] GameObject crushParticle;
	[SerializeField] float crushWaitDuration = 1.5f;

	AudioSource source;
	[SerializeField] AudioClip fallSound;

	// Use this for initialization
	void Start() {
		body = GetComponent<Rigidbody2D>();
		rend = playerObject.GetComponent<Renderer>();
		rend.material.color = color;
		anim = GetComponent<Animator>();
		respawnPos = transform.position;
		col = GetComponent<CircleCollider2D>();
		source = gameObject.AddComponent<AudioSource>();
		source.clip = fallSound;
	}

	// Update is called once per frame
	void Update() {
		if (colorSetCounter < colorSetWaitDuration)
			colorSetCounter += Time.deltaTime;

		if (!canInput) { return; }

		Move();

		if (isFalling) { return; }

		if (isJumping) {
			jumpingTime += Time.deltaTime;
			if (jumpingTime > jumpingDuration) {
				isJumping = false;
				var posForReset = transform.position;
				posForReset.z = 0;
				transform.position = posForReset;
				col.enabled = true;
			}

			var newPos = transform.position;
			var a = jumpHeight / (jumpingDuration * jumpingDuration / 4);
			newPos.z = -a * (jumpingTime - jumpingDuration / 2) * (jumpingTime - jumpingDuration / 2) + jumpHeight;
			transform.position = newPos;
		}

		if (!isJumping && !isFalling) {
			var rayPos = transform.position;
			rayPos.z = -4;
			var ray = new Ray(rayPos, transform.forward);
			if (!Physics.SphereCast(ray, 0.5f, 10.0f) && !isDead) {
				anim.SetTrigger("fall");
				isFalling = true;
				isDead = true;
				source.Play();
			}
		}
	}

	void Move() {

		{ // JoyStick / Keyboard Input
			var x_in = Input.GetAxis("Horizontal");
			var y_in = Input.GetAxis("Vertical");
			var on_x = x_in != 0;
			var on_y = y_in != 0;

			if (on_x || on_y) {
				body.AddForce(new Vector2(x_in, y_in));
				body.velocity = Vector2.ClampMagnitude(body.velocity, maxLength);
			}
		}

		{ // Mouse Input
			var graceWidth = 50;
			var mouse = Input.mousePosition;
			var midX = Screen.width / 2;
			var midY = Screen.height / 2;
			var x_in = mouse.x - midX;
			var y_in = mouse.y - midY;
			var on_x = (midX - graceWidth > mouse.x || mouse.x > midX + graceWidth);
			var on_y = (midY - graceWidth > mouse.y || mouse.y > midY + graceWidth);

			if (on_x || on_y) {
				var vel = new Vector2(x_in, y_in);
				vel *= 0.01f;
				body.velocity = Vector2.ClampMagnitude(vel, maxLength);
			}
		}

		{ // Gyro Input
			var gyro = Input.acceleration;
			var attitude = new Vector2(gyro.x, gyro.y);
			if (attitude.magnitude > 0.001) {
				attitude *= 10.0f;
				body.velocity = Vector2.ClampMagnitude(attitude, maxLength);
			}
		}
	}

	public void SetColor(Color32 new_c) {
		if (colorSetCounter < colorSetWaitDuration) { return; }

		colorSetCounter = 0.0f;

		if (new_c.IsEqualRGB(Color.black)) {
			color = Color.white;
		}
		else if (color.IsEqualRGB(Color.white)) {
			color = new_c;
		}
		else {
			color = Color32.Lerp(color, new_c, 0.5f);
		}
		ApplyColor();
	}

	public Color GetColor() {
		return color;
	}

	void ApplyColor() {
		rend.material.color = color;
	}

	public void StartJump(float duration) {
		isJumping = true;
		jumpingDuration = duration;
		jumpingTime = 0.0f;
		col.enabled = false;
	}

	public void SetRespawn(Vector3 pos) {
		respawnPos = pos;
	}

	public void Respawn() {
		transform.position = respawnPos;
		isFalling = false;
		body.velocity = Vector2.zero;
		rend.enabled = true;
		canInput = true;
		col.enabled = true;
		isDead = false;
	}

	public void Crush() {
		if (!canInput || isDead)
			return;
		Instantiate(crushParticle, transform.position, transform.rotation);
		rend.enabled = false;
		col.enabled = false;
		canInput = false;
		isDead = true;
		Invoke("Respawn", crushWaitDuration);
	}
}