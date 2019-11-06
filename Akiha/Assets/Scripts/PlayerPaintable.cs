using System;
using UnityEngine;

public class PlayerPaintable : MonoBehaviour, IHasColor {
  [SerializeField] Color32 color = Color.white;
  [SerializeField] float colorSetWaitDuration = 0.2f;

  float colorSetCounter = 0.2f;
  Renderer rend;

  public delegate void ApplyColorDelegate(Color32 color);
  ApplyColorDelegate ApplyColor;

  public void SetDelegate(ApplyColorDelegate applier) {
    ApplyColor = applier;
  }

  void Update() {
    colorSetCounter += Time.deltaTime;
  }

  public void SetColor(IHasColor held) {
    if (colorSetWaitDuration > colorSetCounter) {
      return;
    }

    var new_c = held.GetColor();
    colorSetCounter = 0.0f;

    if (new_c.IsEqualRGB(Color.black)) {
      color = Color.white;
    } else if (color.IsEqualRGB(Color.white)) {
      color = new_c;
    } else {
      color = Color32.Lerp(color, new_c, 0.5f);
    }
    ApplyColor(color);
  }

  public Color32 GetColor() {
    return color;
  }
}