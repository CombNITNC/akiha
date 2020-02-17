using System;
using UnityEngine;

public class PlayerPaintable : MonoBehaviour, IHasColor {
  [SerializeField] CMYK color = CMYK.White;
  [SerializeField] float colorSetWaitDuration = 0.2f;

  float colorSetCounter = 0.2f;
  Renderer rend;

  public delegate void ApplyColorDelegate(CMYK color);
  ApplyColorDelegate ApplyColor;

  public void SetDelegate(ApplyColorDelegate applier) {
    ApplyColor = applier;
  }

  void Awake() {
    ApplyColor(color);
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

    if (new_c == CMYK.White) {
      color = CMYK.White;
    } else {
      color = color.Mix(new_c);
    }
    ApplyColor(color);
  }

  public CMYK GetColor() {
    return color;
  }
}