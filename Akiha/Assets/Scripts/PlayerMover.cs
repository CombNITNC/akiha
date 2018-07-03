using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlMode {
  mouse = 0,
  normalized = 1, // JoyStick / Keyboard Input
  gyro = 2,
}

public class PlayerMover : MonoBehaviour {

  [SerializeField] float maxLength = 5.65f;

  Rigidbody2D body;
  ControlMode controlMode = ControlMode.mouse;

  void Start() {
    body = GetComponent<Rigidbody2D>();
  }

  void Update() {
    var x_in = 0f;
    var y_in = 0f;
    switch (controlMode) {
      case ControlMode.mouse:
        {
          var graceWidth = 50;
          var mouse = Input.mousePosition;
          var midX = Screen.width / 2;
          var midY = Screen.height / 2;

          if (midX + graceWidth <= mouse.x) {
            x_in = 1;
          }
          else if (mouse.x <= midX - graceWidth) {
            x_in = -1;
          }

          if (midY + graceWidth <= mouse.y) {
            y_in = 1;
          }
          else if (mouse.y <= midY - graceWidth) {
            y_in = -1;
          }
        }
        break;
      case ControlMode.normalized:
        {
          x_in = Input.GetAxis("Horizontal");
          y_in = Input.GetAxis("Vertical");
        }
        break;
      case ControlMode.gyro:
        {
          var graceWidth = 0.01;
          var gyro = Input.acceleration;

          if (graceWidth < gyro.x) {
            x_in = 1;
          }
          else if (gyro.x < -graceWidth) {
            x_in = -1;
          }

          if (graceWidth < gyro.y) {
            y_in = 1;
          }
          else if (gyro.y < -graceWidth) {
            y_in = -1;
          }
        }
        break;
      default:
        {
          controlMode = ControlMode.mouse;
        }
        break;
    }

    if (x_in != 0 || y_in != 0) {
      var vel = new Vector2(x_in, y_in);
      vel *= 2.3f;
      body.AddForce(vel);
      body.velocity = Vector2.ClampMagnitude(body.velocity, maxLength);
    }
  }

  public void Stop() {
    body.velocity = Vector2.zero;
  }

  public void SetControlMode(ControlMode mode) {
    controlMode = mode;
  }
}