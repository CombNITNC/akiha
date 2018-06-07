using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWiper : MonoBehaviour {
  Texture tex;
  RenderTexture renderTex;
  float alpha = 0.0f;
  bool reEnableListener;
  Camera _cam1, _cam2;

  bool isWaiting = false;
  float waitRate = 1.0f;

  void Update() {
    if (isWaiting) {
      alpha -= Time.deltaTime * waitRate;
      if (alpha <= 0.0f) {
        isWaiting = false;
        CameraCleanup();
      }
      return;
    }
  }

  void OnGUI() {
    GUI.depth = -9999999;

    var tmpColor = GUI.color;
    tmpColor.a = alpha;
    GUI.color = tmpColor;

    if (tex != null) {
      GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);
    }
  }

  void AlphaTimer(float time) {
    waitRate = 1.0f / time;
    alpha = 1.0f;
    isWaiting = true;
  }

  void CameraSetup() {
    this.enabled = true;
    _cam1.gameObject.SetActive(true);
    _cam2.gameObject.SetActive(true);
    var listener = _cam2.GetComponent<AudioListener>();
    if (listener != null) {
      reEnableListener = listener.enabled;
      listener.enabled = false;
    }
  }

  void CameraCleanup() {
    _cam1.targetTexture = null;
    renderTex.Release();

    var listener = _cam2.GetComponent<AudioListener>();
    if (listener != null && reEnableListener) {
      listener.enabled = true;
    }
    _cam1.gameObject.SetActive(false);
    this.enabled = false;
  }

  public void CrossFadePro(Camera cam1, Camera cam2, float time) {
    _cam1 = cam1;
    _cam2 = cam2;
    if (_cam1 == null || _cam1 == null) {
      Debug.LogError("Argument exception. cam1 or cam2 is null.");
      return;
    }
    if (renderTex == null) {
      renderTex = new RenderTexture(Screen.width, Screen.height, 24);
    }
    _cam1.targetTexture = renderTex;
    tex = renderTex;
    CameraSetup();
    AlphaTimer(time);
  }
}