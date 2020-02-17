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

  const float threshold = 0.1f;

  public static bool operator ==(CMYK l, CMYK r) {
    var inY = (Mathf.Abs(l.Y - r.Y) < threshold);
    var inM = (Mathf.Abs(l.M - r.M) < threshold);
    var inC = (Mathf.Abs(l.C - r.C) < threshold);
    var inK = (Mathf.Abs(l.K - r.K) < threshold);
    return inY && inM && inC && inK;
  }
  public static bool operator !=(CMYK l, CMYK r) {
    return !(l == r);
  }

  public CMYK Mix(CMYK r) {
    return new CMYK(Mathf.Clamp01(C + r.C), Mathf.Clamp01(M + r.M), Mathf.Clamp01(Y + r.Y), Mathf.Clamp01(K + r.K));
  }

  public static CMYK from(Color c) {
    var k = 1 - Math.Max(c.r, Math.Max(c.g, c.b));
    return new CMYK((1 - c.r - k) / (1 - k), (1 - c.g - k) / (1 - k), (1 - c.b - k) / (1 - k), k);
  }

  public Color ToColor() {
    return new Color((1 - C) * (1 - K), (1 - M) * (1 - K), (1 - Y) * (1 - K));
  }

  public String ToString() {
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