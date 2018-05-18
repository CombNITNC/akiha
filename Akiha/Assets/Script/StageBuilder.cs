using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBuilder : MonoBehaviour {
  static bool made = false;
  GameObject[] stages = new GameObject[5];

  [SerializeField] GameObject[] presets;
  [SerializeField] GameObject goalForSingle;

  void Awake() {
    if (!made) {
      made = true;
      DontDestroyOnLoad(gameObject);
    }
  }

  public GameObject[] Fetch() { return stages; }

  public void BuildAll() {
    for (int i = 0; i < 5; ++i) {
      stages[i] = presets[i];
    }
  }

  public void BuildSingle(int index) {
    if (!(0 <= index && index <= 4))
      return;
    stages[0] = presets[index];
    stages[1] = goalForSingle;
  }
}