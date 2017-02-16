using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

    private List<GameObject> _itemList = new List<GameObject>();
    private int _quantity;

    public void checkItemDrop(GameObject go, Item item) {
        print("CHECKING ITEM DROP");
        go.SetActive(false);
        _itemList.Add(go);
        var icon = Instantiate(item.icon, transform, false); // create Icon
    }

    public void OnDrop(PointerEventData eventData) {
        print("CHECKING ITEM DROP " + eventData.pointerDrag);
        GameObject go = eventData.pointerPress;
        if (go == null) return; // retorna se não tiver nada em mãos
        Item item = go.GetComponent<Item>();
        if (item != null) {
            // Checa se o objeto é um ITEM
            checkItemDrop(go, item);
        }
    }
}
