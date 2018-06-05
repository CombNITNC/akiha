using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

class GameData {
  public float[] _data = new float[5];
}

public class GameStorageManager : MonoBehaviour {
  [SerializeField] string saveFilePath = "akiha.sav";

  public void Save(float[] data) {
    for (int i = 0; i < 5; ++i) {
      if (data[i] == 0) {
        data[i] = 10000.0f;
      }
    }
    var savingData = new GameData();
    savingData._data = data;
    var json = JsonUtility.ToJson(savingData);
    using(var writer = new StreamWriter(Application.dataPath + "/" + saveFilePath, false)) {
      writer.WriteLine(json);
      writer.Flush();
      writer.Close();
    }
  }

  public void Load(out float[] buf) {
    var info = new FileInfo(Application.dataPath + "/" + saveFilePath);
    if (!info.Exists) {
      var data = new float[5];
      for (int i = 0; i < 5; ++i) {
        if (data[i] == 0) {
          data[i] = 10000.0f;
        }
      }
      buf = data;
      return;
    }
    using(var reader = new StreamReader(info.OpenRead())) {
      var json = reader.ReadToEnd();
      var data = JsonUtility.FromJson<GameData>(json)._data;
      for (int i = 0; i < 5; ++i) {
        if (data[i] == 0) {
          data[i] = 10000.0f;
        }
      }
      buf = data;
    }
  }
}