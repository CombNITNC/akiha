using System;
using UnityEngine;

public class PlayerPaintable : MonoBehaviour, IHasColor {
  [SerializeField] Color32 color = Color.black;
  [SerializeField] float colorSetWaitDuration = 0.2f;

  float colorSetCounter = 0.2f;
  Renderer rend;

  public delegate void ApplyColorDelegate(Color32 color);
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

    if (new_c.IsEqualRGB(Color.black)) {
      color = Color.black;
    } else {
      color = color.Add(new_c);
    }
    ApplyColor(color);
  }

  public Color32 GetColor() {
    return color;
  }
}