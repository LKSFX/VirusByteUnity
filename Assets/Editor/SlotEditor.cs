using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Slot))]
public class SlotEditor : Editor {
    SerializedProperty startWithItem;
    SerializedProperty type;
    SerializedProperty quantity;

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(startWithItem);
        if (startWithItem.boolValue) {
            EditorGUILayout.PropertyField(type);
            EditorGUILayout.PropertyField(quantity);
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable() {
        Debug.Log(target.name + " enabled");
        startWithItem = serializedObject.FindProperty("startWithItem");
        type = serializedObject.FindProperty("type");
        quantity = serializedObject.FindProperty("quantity");
    }
}
