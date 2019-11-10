using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SacredColor {
	const float thresholdF = 0.04f;
	const int thresholdI = 10;

	public static bool IsEqualRGB(this Color lhs, Color rhs) {
		var inR = (Mathf.Abs(lhs.r - rhs.r) < thresholdF);
		var inG = (Mathf.Abs(lhs.g - rhs.g) < thresholdF);
		var inB = (Mathf.Abs(lhs.b - rhs.b) < thresholdF);
		return inR && inG && inB;
	}

	public static bool IsEqualRGB(this Color32 lhs, Color32 rhs) {
		var inR = (Mathf.Abs(lhs.r - rhs.r) < thresholdI);
		var inG = (Mathf.Abs(lhs.g - rhs.g) < thresholdI);
		var inB = (Mathf.Abs(lhs.b - rhs.b) < thresholdI);
		return inR && inG && inB;
	}

	public static Color32 Add(this Color32 lhs, Color32 rhs) {
		var r = (byte) Mathf.Min(lhs.r + rhs.r, 255);
		var g = (byte) Mathf.Min(lhs.g + rhs.g, 255);
		var b = (byte) Mathf.Min(lhs.b + rhs.b, 255);
		return new Color32(r, g, b, 255);
	}

	public static Color32 Sub(this Color32 lhs, Color32 rhs) {
		var r = (byte) Mathf.Max(lhs.r - rhs.r, 0);
		var g = (byte) Mathf.Max(lhs.g - rhs.g, 0);
		var b = (byte) Mathf.Max(lhs.b - rhs.b, 0);
		return new Color32(r, g, b, 255);
	}
}