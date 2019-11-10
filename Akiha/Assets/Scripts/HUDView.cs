using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
public class HUDView : MonoBehaviour {
  [SerializeField] Text signText;
  [SerializeField] Text currentTimeText;
  [SerializeField] Text recordText;
  [SerializeField] Text diffText;
  [SerializeField] bool visibleTime = true;

  float[] _scores;
  int areaIndex = 0;

  public void SetScores(float[] scores) {
    _scores = scores;
  }

  public void SetArea(int loadedIndex) {
    areaIndex = loadedIndex;
    if (0 <= loadedIndex && loadedIndex < _scores.Length) {
      signText.text = "AREA: " + (loadedIndex + 1).ToString();
      var text = _scores[loadedIndex].ToString("00.0000");
      recordText.text = text;
    } else {
      signText.text = "AREA: ?";
      diffText.text = "Something Went Wrong";
    }
  }

  public void SetTime(float elapsed) {
    if (visibleTime) {
      currentTimeText.text = elapsed.ToString("00.0000");
    }

    if (_scores[areaIndex] == 10000.0f) {
      diffText.text = "First Record";
    } else {
      var diff = elapsed - _scores[areaIndex];
      if (diff < 0) {
        diffText.text = "-" + (-diff).ToString("00.000");
        diffText.color = Color.green;
      } else {
        diffText.text = "+" + diff.ToString("00.000");
        diffText.color = Color.red;
      }
    }
  }
}