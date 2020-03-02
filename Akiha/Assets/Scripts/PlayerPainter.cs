using UnityEngine;

public class PlayerPainter : MonoBehaviour {
  const float colorSetWaitDuration = 0.2f;

  CMYK color;

  public CMYK Color {
    get { return color; }
    set {
      if (colorSetWaitDuration > colorSetCounter) {
        return;
      }

      colorSetCounter = 0.0f;

      if (value == CMYK.White) {
        color = CMYK.White;
      } else {
        color = Color.Mix(value);
      }
      var rgb = color.ToColor();
      rend.materials[rend.materials.Length - 1].SetColor("_TintColor", rgb);
      rend.materials[rend.materials.Length - 1].color = rgb;
    }
  }

  float colorSetCounter = 0.2f;
  Renderer rend;

  public static PlayerPainter Attach(GameObject go, Renderer renderer) {
    var painter = go.AddComponent<PlayerPainter>();
    painter.rend = renderer;
    return painter;
  }

  void Start() {
    Color = CMYK.White;
  }

  void Update() {
    colorSetCounter += Time.deltaTime;
  }
}