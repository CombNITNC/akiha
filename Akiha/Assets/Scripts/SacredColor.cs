using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasColor {
	Color32 GetColor();
}

public static class SacredColor {
	const float thresholdF = 0.02f;
	const int thresholdI = 5;

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
}