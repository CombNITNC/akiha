using System.Collections;
using UnityEngine;

public interface IHasColor {
	CMYK GetColor();
}

public interface ICollideWithColor {
	void CollideWith(CMYK color, PlayerController player);
}

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerController : MonoBehaviour {
	[SerializeField] GameObject playerObject = null;
	[SerializeField] float maxLength = 5.65f;
	[SerializeField] float jumpHeight = 2.0f;
	[SerializeField] GameObject crushParticle = null;
	[SerializeField] float crushWaitDuration = 1.5f;
	[SerializeField] AudioClip fallSound = null;

	enum PhysicsState {
		OnFloor,
		Jumping,
		Falling,
		Dead,
	}
	PhysicsState state = PhysicsState.OnFloor;

	Rigidbody2D body;
	Renderer rend;
	Animator anim;
	CircleCollider2D col;
	Vector3 respawnPos;
	AudioSource source;
	PlayerMover mover;
	PlayerPainter painter;

	// Use this for initialization
	void Awake() {
		body = GetComponent<Rigidbody2D>();
		rend = playerObject.GetComponent<Renderer>();
		anim = GetComponent<Animator>();
		respawnPos = transform.position;
		col = GetComponent<CircleCollider2D>();
		source = gameObject.AddComponent<AudioSource>();
		mover = PlayerMover.Attach(gameObject, maxLength);
		painter = PlayerPainter.Attach(gameObject, rend);
		source.clip = fallSound;
	}

	// Update is called once per frame
	void Update() {
		if (state == PhysicsState.OnFloor) {
			DetectFalling();
		}
	}

	void DetectFalling() {
		var rayPos = transform.position;
		rayPos.z = -4;
		var ray = new Ray(rayPos, transform.forward);
		if (!Physics.SphereCast(ray, 0.5f, 10.0f)) {
			state = PhysicsState.Falling;
			anim.SetTrigger("fall");
			source.Play();
		}
	}

	IEnumerator JumpWork(float duration) {
		state = PhysicsState.Jumping;
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
		state = PhysicsState.OnFloor;
		yield break;
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (state != PhysicsState.OnFloor) return;

		var collider = col.GetComponent<ICollideWithColor>();
		if (collider != null) {
			collider.CollideWith(painter.Color, this);
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (state != PhysicsState.OnFloor) return;

		var breakable = col.gameObject.GetComponent<ICollideWithColor>();
		if (breakable != null) {
			breakable.CollideWith(painter.Color, this);
		}
	}

	public void StartJump(float duration) {
		StartCoroutine(JumpWork(duration));
	}

	public void SetRespawn(Vector3 pos) {
		if (state != PhysicsState.OnFloor)
			return;

		respawnPos = pos;
	}

	public void Respawn() {
		transform.position = respawnPos;
		state = PhysicsState.OnFloor;
		body.velocity = Vector2.zero;
		rend.enabled = true;
		mover.enabled = true;
		col.enabled = true;
	}

	public void Crush() {
		if (state != PhysicsState.OnFloor)
			return;

		Instantiate(crushParticle, transform.position, transform.rotation);
		rend.enabled = false;
		col.enabled = false;
		mover.enabled = false;
		state = PhysicsState.Dead;
		Invoke("Respawn", crushWaitDuration);
	}

	public void SetControlMode(ControlMode mode) {
		mover.SetControlMode(mode);
	}

	public void Paint(CMYK color) {
		painter.Color = color;
	}

	public void Finish(Vector3 finishPos) {
		mover.Stop(finishPos);
		mover.enabled = false;
		anim.SetTrigger("finish");
	}
}