using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Quando o jogo está em pausa o item não pode ser pego.
/// </summary>
public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    private List<Item> _itemList = new List<Item>();
    private GameObject _icon;
    private GameObject _currentDrag;
    private int _quantity;

    /// <summary>
    /// Retorna verdadeiro se o item foi inserido no slot com sucesso.
    /// </summary>
    /// <param name="go"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool checkItemDrop(GameObject go) {
        if (go == null) return false; // não aceita objeto NULO
        print(go + " dropped over slot.");
        return insertItem(go.GetComponent<Item>(), go);
    }

    private bool insertItem(Item item, GameObject go) {
        if (item == null) return false; // Objeto não é do tipo ITEM, retorna falso
        if (_itemList.Count > 0 && item.type != _itemList[0].type) return false; // Itens de tipos diferentes, retorna falso
        go.SetActive(false); // deixa objeto inativo, isto é, invisível na tela e sem receber inputs
        _itemList.Add(item);
        if (_itemList.Count == 1) // Só adiciona o icone quando o slot estiver previamente vázio
            _icon = Instantiate(item.icon, transform, false); // cria e mostra Icone
        return true;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (_itemList.Count > 0) { 
            // Contém Item
            Item item = _itemList[0];
            _itemList.Remove(item);
            GameObject go = item.gameObject;
            go.SetActive(true);
            ExecuteEvents.Execute(go, eventData, ExecuteEvents.pointerDownHandler);
            _currentDrag = go;
            if (_itemList.Count == 0)
                Destroy(_icon);
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (_currentDrag != null) {
            ExecuteEvents.Execute(_currentDrag, eventData, ExecuteEvents.pointerUpHandler);
            _currentDrag = null;
        }
    }
}
