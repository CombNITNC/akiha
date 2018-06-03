using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStory : MonoBehaviour {
  GameObject[] stages = new GameObject[5];

  [SerializeField] GameObject[] presets = new GameObject[5];
  [SerializeField] GameObject goalForSingle;

  public GameObject[] Fetch() { return stages; }

  public void All() {
    for (int i = 0; i < 5; ++i) {
      stages[i] = presets[i];
    }
  }

  public void Single(int index) {
    if (!(0 <= index && index <= 4))
      return;
    stages[0] = presets[index];
    stages[1] = goalForSingle;
  }
}