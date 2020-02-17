using System.Collections;
using UnityEngine;

public class ScreenWiper : MonoBehaviour {
  RenderTexture renderTex = null;
  float alpha = 0.0f;
  bool reEnableListener = false;

  void OnGUI() {
    GUI.depth = -9999999;

    var tmpColor = GUI.color;
    tmpColor.a = alpha;
    GUI.color = tmpColor;

    if (renderTex != null) {
      GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), renderTex);
    }
  }

  IEnumerator AlphaTimer(Camera cam1, Camera cam2, float time) {
    CameraSetup(cam1, cam2);

    var elapsed = 0.0f;
    alpha = 1.0f;
    while (elapsed < time) {
      alpha = 1.0f - elapsed / time;
      yield return null;
      elapsed += Time.deltaTime;
    }
    alpha = 0.0f;

    CameraCleanup(cam1, cam2);
  }

  void CameraSetup(Camera cam1, Camera cam2) {
    if (renderTex == null) {
      renderTex = new RenderTexture(Screen.width, Screen.height, 24);
    }
    cam1.targetTexture = renderTex;
    cam1.Render();

    cam1.gameObject.SetActive(true);
    cam2.gameObject.SetActive(true);
    var listener = cam2.GetComponent<AudioListener>();
    if (listener != null) {
      reEnableListener = listener.enabled;
      listener.enabled = false;
    }
  }

  void CameraCleanup(Camera cam1, Camera cam2) {
    cam1.targetTexture = null;
    renderTex.Release();
    renderTex = null;

    var listener = cam2.GetComponent<AudioListener>();
    if (listener != null && reEnableListener) {
      listener.enabled = true;
    }
    cam1.gameObject.SetActive(false);
  }

  public void CrossFadePro(Camera cam1, Camera cam2, float time) {
    if (cam1 == null || cam1 == null) {
      Debug.LogError("Argument exception. cam1 or cam2 is null.");
      return;
    }

    StartCoroutine(AlphaTimer(cam1, cam2, time));
  }
}