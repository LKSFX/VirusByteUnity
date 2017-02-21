using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Quando o jogo está em pausa o item não pode ser pego.
/// </summary>
[RequireComponent(typeof(Image))]
public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public bool hasItem {
        get { return _itemList.Count > 0; }
    }

    private List<Item> _itemList = new List<Item>();
    private GameObject _icon;
    private GameObject _currentDrag;
    private Image _image; // Slot sprite

    private void Awake() {
        _image = GetComponent<Image>();
        updateState();
    }

    /// <summary>
    /// Verifica se há algum Item neste Slot; torna-se ativo caso haja, do contrário torna-se inativo.
    /// </summary>
    private void updateState() {
        bool active = hasItem; // ativo quando há itens
        _image.raycastTarget = active;
    }

    /// <summary>
    /// Retorna verdadeiro se o item foi inserido no slot com sucesso.
    /// </summary>
    /// <param name="go"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool checkItemDrop(GameObject go) {
        if (go == null) return false; // não aceita objeto NULO
        //print(go + " dropped over slot.");
        return insertItem(go.GetComponent<Item>(), go);
    }

    private bool insertItem(Item item, GameObject go) {
        if (item == null) return false; // Objeto não é do tipo ITEM, retorna falso
        if (_itemList.Count > 0 && item.type != _itemList[0].type) return false; // Itens de tipos diferentes, retorna falso
        go.SetActive(false); // deixa objeto inativo, isto é, invisível na tela e sem receber inputs
        item.transform.position = new Vector3(transform.position.x, transform.position.y, 0); // Ajusta posição de inércia
        _itemList.Add(item);
        if (_itemList.Count == 1) // Só adiciona o icone quando o slot estiver previamente vázio
            _icon = Instantiate(item.icon, transform, false); // cria e mostra Icone
        updateState();
        return true;
    }

    /// <summary>
    /// Retira Item, quando não está vázio
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData) {
        //print("Pointer down over Slot");
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
            updateState();
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (_currentDrag != null) {
            ExecuteEvents.Execute(_currentDrag, eventData, ExecuteEvents.pointerUpHandler);
            _currentDrag = null;
        }
    }

    GameManager.Action _onPause;
    GameManager.Action _onUnpause;

    private void OnEnable() {
        GameManager.instance.addOnPauseAction(_onPause = () => { gameObject.layer = LayerMask.NameToLayer("Ignore Raycast"); });
        GameManager.instance.addOnUnpauseAction(_onUnpause = () => { gameObject.layer = LayerMask.NameToLayer("UI"); });
    }

    private void OnDisable() {
        if (GameManager.instance == null)
            return; // Jovo já foi fechado e GameManager já foi destruido.
        GameManager.instance.removeOnPauseAction(_onPause);
        GameManager.instance.removeOnUpauseAction(_onUnpause);
    }
}
