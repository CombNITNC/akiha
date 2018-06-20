using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlMode {
  mouse = 0,
  normalized = 1, // JoyStick / Keyboard Input
  gyro = 2,
}

public class PlayerMover : MonoBehaviour {

  ControlMode controlMode = 0;

  void Update() {
    switch (controlMode) {
      case mouse:
        {
          var graceWidth = 50;
          var mouse = Input.mousePosition;
          var midX = Screen.width / 2;
          var midY = Screen.height / 2;

          var x_in = 0;
          if (midX - graceWidth <= mouse.x) {
            x_in = -1;
          }
          else if (mouse.x <= midX - graceWidth) {
            x_in = 1;
          }

          var y_in = 0;
          if (midY - graceWidth <= mouse.y) {
            y_in = -1;
          }
          else if (mouse.y <= midY - graceWidth) {
            y_in = 1;
          }

          if (x_in != 0 || y_in != 0) {
            var vel = new Vector2(x_in, y_in);
            vel *= 2.3f;
            body.AddForce(vel);
            body.velocity = Vector2.ClampMagnitude(body.velocity, maxLength);
          }
        }
        break;
      case normalized:
        {
          var x_in = Input.GetAxis("Horizontal");
          var y_in = Input.GetAxis("Vertical");

          if (x_in != 0 || y_in != 0) {
            var vel = new Vector2(x_in, y_in);
            vel *= 2.3f;
            body.AddForce(vel);
            body.velocity = Vector2.ClampMagnitude(body.velocity, maxLength);
          }
        }
        break;
      case gyro:
        {
          var gyro = Input.acceleration;
          var attitude = new Vector2(gyro.x, gyro.y);
          if (attitude.magnitude > 0.001) {
            attitude *= 10.0f;
            body.velocity = Vector2.ClampMagnitude(attitude, maxLength);
          }
        }
        break;
      default:
        {
          controlMode = 0;
        }
        break;
    }
  }

  public void SetControlMode(ControlMode mode) {
    controlMode = mode;
  }
}