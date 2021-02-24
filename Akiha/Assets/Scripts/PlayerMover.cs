using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ControlMode {
  mouse = 0,
  normalized = 1, // JoyStick / Keyboard Input
  gyro = 2,
}

public class PlayerMover : MonoBehaviour {
  float maxLength = 5.65f;

  Rigidbody2D body;
  ControlMode controlMode = ControlMode.mouse;

  bool pausing = true;

  InputAction moveAction = null;

  public static PlayerMover Attach(GameObject go, float MaxLength = 5.65f) {
    var mover = go.AddComponent<PlayerMover>();
    mover.maxLength = MaxLength;
    mover.SetControlMode((ControlMode) PlayerPrefs.GetInt("ControlMode", 0));
    return mover;
  }

  void Start() {
    body = GetComponent<Rigidbody2D>();
    var gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    gc.Continue.AddListener(OnContinue);
    gc.Pause.AddListener(OnPause);

    moveAction = new InputAction("move", type: InputActionType.PassThrough, binding: "<Gamepad>/leftStick");
    moveAction.AddBinding("<Accelerometer>/acceleration");
    moveAction.AddCompositeBinding("Dpad")
      .With("Up", "<Keyboard>/w")
      .With("Down", "<Keyboard>/s")
      .With("Left", "<Keyboard>/a")
      .With("Right", "<Keyboard>/d")
      .With("Up", "<Keyboard>/uparrow")
      .With("Down", "<Keyboard>/downarrow")
      .With("Left", "<Keyboard>/leftarrow")
      .With("Right", "<Keyboard>/rightarrow");
    moveAction.Enable();
  }

  void Update() {
    if (pausing) { return; }
    var raw = moveAction.ReadValue<Vector2>();

    if (raw.x != 0 || raw.y != 0) {
      raw *= 2.3f;
      body.AddForce(raw);
      body.velocity = Vector2.ClampMagnitude(body.velocity, maxLength);
    }
  }

  IEnumerator StopWork(Vector3 pos) {
    float count = 0f;
    var before = transform.position;
    while (count < 1f) {
      count += Time.deltaTime;
      transform.position = Vector3.Lerp(before, pos, count);
      yield return null;
    }
    yield break;
  }

  public void Stop(Vector3 pos) {
    body.velocity = Vector2.zero;
    StartCoroutine(StopWork(pos));
  }

  public void SetControlMode(ControlMode mode) {
    controlMode = mode;
  }

  public void OnContinue() {
    body.velocity = Vector2.zero;
    pausing = false;
  }
  public void OnPause() {
    pausing = true;
  }
}
