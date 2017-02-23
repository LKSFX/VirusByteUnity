using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Slot))]
public class SlotEditor : Editor {
    SerializedProperty startWidthItem;

    public override void OnInspectorGUI() {
        var slot = target as Slot;
        slot.startWidthItem = GUILayout.Toggle(slot.startWidthItem, "StartWithItem");
        if (slot.startWidthItem) {
            slot.type = (Item.ItemType)EditorGUILayout.EnumPopup("Type: ", slot.type);
            //EditorGUILayout.LabelField("A quantidade de itens que o slot terá ao carregar a fase.");
            slot.quantity = EditorGUILayout.IntField("Quantity: ", slot.quantity);
        }
    }

    private void OnEnable() {
        Debug.Log(target.name + " enabled");

    }
}
