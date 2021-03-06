using UnityEngine;

public class GameStory : MonoBehaviour {
  GameObject[] stages = new GameObject[5];

  [SerializeField] GameObject[] presets = new GameObject[5];
  [SerializeField] GameObject goalForSingle = null;

  public GameObject[] Fetch() { return stages; }

  public void All() {
    for (int i = 0; i < presets.Length; ++i) {
      if (presets[i] == null) {
        Debug.LogError("The stage is invalid at " + i);
        stages[i] = goalForSingle;
        return;
      }
      stages[i] = presets[i];
    }
  }

  public void Single(int index) {
    if (!(0 <= index && index <= 4)) {
      Debug.LogError(index + " is invalid stage index!");
      return;
    }
    stages[0] = presets[index];
    stages[1] = goalForSingle;
  }

  public bool Invalid() {
    return stages[0] == null || stages[1] == null;
  }
}