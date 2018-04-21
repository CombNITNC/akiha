using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	Rigidbody2D body;
	[SerializeField] Color32 color = Color.white;
	[SerializeField] GameObject playerObject;
	Renderer rend;
	Animator anim;

	bool isJumping = false;
	float jumpingTime = 0.0f;
	float jumpingDuration = 0.0f;
	[SerializeField] float jumpHeight = 2.0f;

	bool isFalling = false;
	Vector3 respawnPos;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D>();
		rend = playerObject.GetComponent<Renderer>();
		rend.material.color = color;
		anim = GetComponent<Animator>();
		respawnPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		var x_in = Input.GetAxis("Horizontal");
		var y_in = Input.GetAxis("Vertical");
		if (x_in != 0 && y_in != 0) {
			x_in *= 0.707f;
			y_in *= 0.707f;
		}
		if (x_in != 0 || y_in != 0) {
			body.AddForce(new Vector2(x_in, y_in));
		}

		if (isFalling) { return; }

		if (isJumping) {
			jumpingTime += Time.deltaTime;
			if (jumpingTime > jumpingDuration) {
				isJumping = false;
				var posForReset = transform.position;
				posForReset.z = 0;
				transform.position = posForReset;
				GetComponent<CircleCollider2D>().enabled = true;
			}

			var newPos = transform.position;
			var a = jumpHeight / (jumpingDuration * jumpingDuration / 4);
			newPos.z = -a * (jumpingTime - jumpingDuration / 2) * (jumpingTime - jumpingDuration / 2) + jumpHeight;
			transform.position = newPos;
		}

		Debug.DrawRay(transform.position, transform.forward * 10);

		if (!isJumping && !isFalling) {
			var ray = new Ray(transform.position, transform.forward);
			if (!Physics.SphereCast(ray, 0.5f, 10.0f)) {
				anim.SetTrigger("fall");
				isFalling = true;
			}
		}
	}

	public void SetColor(Color32 new_c) {
		if (color == Color.white) {
			color = new_c;
		} else if (new_c == Color.black) {
			color = Color.white;
		} else {
			color = Color32.Lerp(color, new_c, 0.5f);
		}
		rend.material.color = color;
	}

	public Color GetColor() {
		return color;
	}

	public void StartJump(float duration) {
		isJumping = true;
		jumpingDuration = duration;
		jumpingTime = 0.0f;
		GetComponent<CircleCollider2D>().enabled = false;
	}

	public void SetRespawn(Vector3 pos) {
		respawnPos = pos;
	}

	public void Respawn() {
		transform.position = respawnPos;
		isFalling = false;
		body.velocity = Vector2.zero;
	}
}
