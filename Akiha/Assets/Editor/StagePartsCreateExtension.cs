using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StagePartsCreateExtension : Editor {
  [MenuItem("GameObject/Create Other/Add Color Barrier")]
  static void AddColorBarrier() {
    var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    DestroyImmediate(cube.GetComponent<BoxCollider>());
    cube.AddComponent<ColorBarrier>();
  }

  [MenuItem("GameObject/Create Other/Add Color Deployer")]
  static void AddColorDeployer() {
    UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/StagePrefab/Parts/ColorDeployerModel.fbx", typeof(GameObject));
    var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
    obj.transform.Rotate(-180f, 0f, 0f);
    obj.transform.localScale = Vector3.one;
  }

  [MenuItem("GameObject/Create Other/Add Jump Ramp")]
  static void AddJumpRamp() {
    UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/StagePrefab/Parts/JumpRampModel.fbx", typeof(GameObject));
    var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
    obj.transform.Rotate(-90f, 0f, 0f);
  }

  [MenuItem("GameObject/Create Other/Add Laser")]
  static void AddLaser() {
    UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/StagePrefab/Parts/LaserBody.prefab", typeof(GameObject));
    Instantiate(prefab, Vector3.zero, Quaternion.identity);
  }

  [MenuItem("GameObject/Create Other/Add One Way Wall")]
  static void AddOneWayWall() {
    UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/StagePrefab/Parts/OneWayWall.prefab", typeof(GameObject));
    Instantiate(prefab, Vector3.zero, Quaternion.identity);
  }

  [MenuItem("GameObject/Create Other/Add Respawn Point")]
  static void AddRespawnPoint() {
    UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/StagePrefab/Parts/RespawnPoint.prefab", typeof(GameObject));
    var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
    obj.transform.Rotate(52.7f, 0f, 45f);
  }

  [MenuItem("GameObject/Create Other/Add Spikes")]
  static void AddSpikes() {
    UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/StagePrefab/Parts/SpikesModel.fbx", typeof(GameObject));
    var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
    obj.AddComponent<Spikes>();
    obj.transform.Rotate(-180f, 0f, 0f);
    obj.transform.localScale = Vector3.one;
  }
}