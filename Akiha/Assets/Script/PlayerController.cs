using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	Rigidbody2D body;
	[SerializeField] Color color = Color.white;
	[SerializeField] GameObject playerObject;
	Renderer rend;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D>();
		rend = playerObject.GetComponent<Renderer>();
		rend.material.color = color;
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
	}

	public void SetColor(Color new_c) {
		color = new_c;
		rend.material.color = color;
	}

	public Color GetColor() {
		return color;
	}
}
