using System;
using UnityEngine;

[Serializable]
public struct CMYK {
  public float C;
  public float M;
  public float Y;
  public float K;

  public CMYK(float c, float m, float y, float k = 0) {
    C = c;
    M = m;
    Y = y;
    K = k;
  }

  public static readonly CMYK Black = new CMYK(0, 0, 0, 1);
  public static readonly CMYK White = new CMYK(0, 0, 0, 0);
  public static readonly CMYK Red = new CMYK(0, 1, 1);

  const float threshold = 0.2f;

  public static bool operator ==(CMYK l, CMYK r) {
    if (0.99 <= l.K) { // If l is black
      var rK = r.C * r.M * r.Y;
      return Mathf.Abs(l.K - rK) < threshold;
    }
    if (0.99 <= r.K) { // If r is black
      var lK = l.C * l.M * l.Y;
      return Mathf.Abs(lK - r.K) < threshold;
    }

    var inC = (Mathf.Abs(l.C - r.C) < threshold);
    var inM = (Mathf.Abs(l.M - r.M) < threshold);
    var inY = (Mathf.Abs(l.Y - r.Y) < threshold);
    var inK = (Mathf.Abs(l.K - r.K) < threshold);
    return inC && inM && inY && inK;
  }
  public static bool operator !=(CMYK l, CMYK r) {
    return !(l == r);
  }

  static float SafetyAdd(float a, float b) {
    return Mathf.Clamp01(Mathf.Clamp01(a) + Mathf.Clamp01(b));
  }

  public CMYK Mix(CMYK r) {
    return new CMYK(SafetyAdd(C, r.C), SafetyAdd(M, r.M), SafetyAdd(Y, r.Y), SafetyAdd(K, r.K));
  }

  public static CMYK from(Color c) {
    var k = 1 - Math.Max(c.r, Math.Max(c.g, c.b));
    if (0.99 <= k) return CMYK.Black;
    return new CMYK((1 - c.r - k) / (1 - k), (1 - c.g - k) / (1 - k), (1 - c.b - k) / (1 - k), k);
  }

  public Color ToColor() {
    return new Color((1 - C) * (1 - K), (1 - M) * (1 - K), (1 - Y) * (1 - K));
  }

  public override String ToString() {
    return String.Format("{0} {1} {2} {3}", C, M, Y, K);
  }

  public override bool Equals(object obj) {
    return obj is CMYK cmyk &&
      C == cmyk.C &&
      M == cmyk.M &&
      Y == cmyk.Y &&
      K == cmyk.K;
  }

  public override int GetHashCode() {
    int hashCode = -492570696;
    hashCode = hashCode * -1521134295 + C.GetHashCode();
    hashCode = hashCode * -1521134295 + M.GetHashCode();
    hashCode = hashCode * -1521134295 + Y.GetHashCode();
    hashCode = hashCode * -1521134295 + K.GetHashCode();
    return hashCode;
  }
}