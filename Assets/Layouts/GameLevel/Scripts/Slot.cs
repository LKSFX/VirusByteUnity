using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

    private GameObject _item;
    private int _quantity;

    public void checkItemDrop(GameObject go) {
        print("CHECKING ITEM DROP");
        go.SetActive(false);
        _item = go;
    }

    public void OnDrop(PointerEventData eventData) {
        print("CHECKING ITEM DROP " + eventData.pointerDrag);
        GameObject go = eventData.pointerPress;
        if (go == null) return; // retorna se não tiver nada em mãos
        ItemGrabber ig = go.GetComponent<ItemGrabber>();
        if (ig != null) {
            checkItemDrop(go);
        }
    }
}
