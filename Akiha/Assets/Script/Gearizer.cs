using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Gearizer : MonoBehaviour {
  [SerializeField] int teeth = 32;

  void Start() {
    Mesh mesh = GetComponent<MeshFilter>().mesh;
    Vector3[] vertexes = new Vector3[teeth * 6];
    float c = 0f, offset = 0f;
    for (int i = 0; i < teeth * 6; i += 6) {
      if (i % 12 == 0)
        offset = 0.5f;
      else
        offset = 0f;

      float x0 = Mathf.Cos(c * Mathf.Deg2Rad) * (2.5f + offset);
      float z0 = Mathf.Sin(c * Mathf.Deg2Rad) * (2.5f + offset);
      float x0b = Mathf.Cos(c * Mathf.Deg2Rad) * 1.5f;
      float z0b = Mathf.Sin(c * Mathf.Deg2Rad) * 1.5f;
      float x1 = Mathf.Cos((c + 360f / teeth) * Mathf.Deg2Rad) * (2.5f + offset);
      float z1 = Mathf.Sin((c + 360f / teeth) * Mathf.Deg2Rad) * (2.5f + offset);
      float x1b = Mathf.Cos((c + 360f / teeth) * Mathf.Deg2Rad) * 1.5f;
      float z1b = Mathf.Sin((c + 360f / teeth) * Mathf.Deg2Rad) * 1.5f;
      vertexes[i] = new Vector3(x0, z0, 0);
      vertexes[i + 1] = new Vector3(x1b, z1b, 0);
      vertexes[i + 2] = new Vector3(x1, z1, 0);
      vertexes[i + 3] = new Vector3(x0, z0, 0);
      vertexes[i + 4] = new Vector3(x0b, z0b, 0);
      vertexes[i + 5] = new Vector3(x1b, z1b, 0);
      c += 360f / teeth;
    }

    mesh.vertices = vertexes;
    // TODO: UV
    int[] triangles = new int[mesh.vertices.Length];
    for (int i = 0; i < mesh.vertices.Length; ++i) {
      triangles[i] = i;
    }
    mesh.triangles = triangles;
    mesh.RecalculateNormals();
    mesh.RecalculateBounds();
    GetComponent<MeshFilter>().sharedMesh = mesh;
  }
}