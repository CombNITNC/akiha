using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage), typeof(Mask))]
public class UITransition : MonoBehaviour {
  [SerializeField] Texture maskImage;
  [SerializeField] RenderTexture dst;
  [SerializeField] Shader shader;
  Material renderMat;
  [SerializeField, Range(0, 1)] float ratio = 0f;

  void Start() {
    renderMat = new Material(shader);
  }

  void Update() {
    mat.SetFloat("_Ratio", ratio);
    Graphics.Blit(maskImage, dst, renderMat);

    var mask = GetComponent<Mask>();
    mask.enabled = false;
    mask.enabled = true;
  }

  void OnDestroy() {
    DestroyImmediate(renderMat);
  }

  IEnumerator FadeInWork(float duration, UnityAction act) {
    float end = Time.timeSinceLevelLoad + duration * (1f - ratio);
    var endFrame = new WaitForEndOfFrame();

    do {
      ratio = 1f - ((end - Time.timeSinceLevelLoad) / duration);
      yield return endFrame;
    } while (Time.timeSinceLevelLoad <= end);
    ratio = 1f;

    if (act != null) {
      act();
    }
  }

  IEnumerator FadeOutWork(float duration, UnityAction act) {
    float end = Time.timeSinceLevelLoad + duration * ratio;
    var endFrame = new WaitForEndOfFrame();

    do {
      ratio = (end - Time.timeSinceLevelLoad) / duration;
      yield return endFrame;
    } while (Time.timeSinceLevelLoad <= end);
    ratio = 0f;

    if (act != null) {
      act();
    }
  }

  public float GetRatio() { return ratio; }

  public void FadeIn(float duration, UnityAction act) {
    StopAllCoroutines();
    StartCoroutine(FadeInWork(duration, act));
  }

  public void FadeOut(float duration, UnityAction act) {
    StopAllCoroutines();
    StartCoroutine(FadeOutWork(duration, act));
  }
}