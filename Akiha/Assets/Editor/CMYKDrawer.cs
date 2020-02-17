using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CMYK))]
public class CMYKDrawer : PropertyDrawer {
  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
    if (property.hasMultipleDifferentValues) return;
    var C = property.FindPropertyRelative("C");
    var M = property.FindPropertyRelative("M");
    var Y = property.FindPropertyRelative("Y");
    var K = property.FindPropertyRelative("K");
    if (C == null || M == null || Y == null || K == null) return;
    var cmyk = new CMYK(C.floatValue, M.floatValue, Y.floatValue, K.floatValue);

    var newColor = EditorGUI.ColorField(position, label.text, cmyk.ToColor());
    cmyk = CMYK.from(newColor);

    C.floatValue = cmyk.C;
    M.floatValue = cmyk.M;
    Y.floatValue = cmyk.Y;
    K.floatValue = cmyk.K;
  }
}