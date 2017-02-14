using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(RangeEx))]
public class RangeExDrawer : PropertyDrawer {
    private float value;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var rangeAttribute = (RangeEx)base.attribute;

        if (property.propertyType == SerializedPropertyType.Float) {
            value = EditorGUI.Slider(position, value, rangeAttribute.min, rangeAttribute.max);

            value = (value / rangeAttribute.step) * rangeAttribute.step;
            value = Mathf.Round(value * 100f) / 100f;
            property.floatValue = value;
        } else {
            EditorGUI.LabelField(position, label.text, "Usage Range with float or int.");
        }
    }
}
