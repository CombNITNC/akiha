using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SacredColor {
	const float threshold = 0.008f;

	public static bool IsEqualRGB(this Color lhs, Color rhs) {
		var inR = (Math.Abs(lhs.r - rhs.r) < threshold);
		var inG = (Math.Abs(lhs.g - rhs.g) < threshold);
		var inB = (Math.Abs(lhs.b - rhs.b) < threshold);
		return inR && inG && inB;
	}
}