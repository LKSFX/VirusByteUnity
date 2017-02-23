using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Quando o jogo está em pausa o item não pode ser pego.
/// </summary>
[RequireComponent(typeof(Image), typeof(CanvasGroup))]
public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IPointerExitHandler {

    [ReadOnlyWhenPlaying]
    public bool startWithItem;
    [ReadOnlyWhenPlaying]
    public Item.ItemType type;
    [ReadOnlyWhenPlaying]
    public int quantity = 1;

    private Dictionary<Item.ItemType, String> _prefabList = new Dictionary<Item.ItemType, String>() {
        { Item.ItemType.BOMB, "Items/Bomb"}
    };

    public bool hasItem {
        get { return _itemList.Count > 0; }
    }

    private List<Item> _itemList = new List<Item>();
    private GameObject _icon;
    private GameObject _currentDrag;
    private Image _image; // Slot sprite
    private CanvasGroup _cGroup;
    private CircleCollider2D _collider;
    private Coroutine _fadeRoutine;
    float _minOpacity = 0.3f, _maxOpacity = 1f;

    private void Awake() {
        _image = GetComponent<Image>();
        _cGroup = GetComponent<CanvasGroup>();
        _collider = GetComponent<CircleCollider2D>();
        updateState();
        fadeOut();
        debugItem();
    }

    private void debugItem() {
        if (startWithItem) {
            GameObject prefab = Resources.Load(_prefabList[type]) as GameObject;
            if (prefab != null) {
                int total = Mathf.Abs(quantity);
                GameObject go;
                for (int i = 0; i < total; i++) {
                    go = Instantiate(prefab);
                    checkItemDrop(go);
                }
            }
        }
    }

    /// <summary>
    /// Verifica se há algum Item neste Slot; torna-se ativo caso haja, do contrário torna-se inativo.
    /// </summary>
    private void updateState() {
        bool active = hasItem; // ativo quando há itens
        //_cGroup.blocksRaycasts = active;
        _image.raycastTarget = active;
        // define tamanho do collider
        if (active) {
            // Quando este slot tiver algum Item terá também um raio de detecção de touch maior
            _collider.radius = ((RectTransform)transform).rect.width / 3;
        } else {
            _collider.radius = ((RectTransform)transform).rect.width / 4;
        }
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
        item.onSlotEnter(); // Deixa o item no tamanho máximo de drag
        
        _itemList.Add(item);
        if (_itemList.Count == 1) // Só adiciona o icone quando o slot estiver previamente vázio
            _icon = Instantiate(item.icon, transform, false); // cria e mostra Icone
        updateState();
        fadeIn(1f);
        Invoke("fadeOut", 1f);
        return true;
    }

    #region Implementações do EVENTSYSTEMS
    /// <summary>
    /// Retira Item, quando não está vázio
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData) {
        //print("Pointer down over Slot");
        if (hasItem)
            Vibrator.vibrate(15);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        
    }

    public void OnDrag(PointerEventData eventData) {

    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.dragging)
            grabItem(eventData);
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (_currentDrag != null) {
            ExecuteEvents.Execute(_currentDrag, eventData, ExecuteEvents.pointerUpHandler);
            _currentDrag = null;
        }
    }
    #endregion

    public void grabItem(PointerEventData eventData) {
        if (_itemList.Count > 0) {
            // Contém Item
            Item item = _itemList[0];
            _itemList.Remove(item);
            GameObject go = item.gameObject;
            go.SetActive(true);
            go.transform.position = new Vector3(transform.position.x, transform.position.y, 0); // Ajusta posição de spawn
            ExecuteEvents.Execute(go, eventData, ExecuteEvents.pointerDownHandler);
            _currentDrag = go;
            if (_itemList.Count == 0)
                Destroy(_icon);
            updateState();
        }
    }

    GameManager.Action _onPause;
    GameManager.Action _onUnpause;

    private void OnEnable() {
        GameManager.instance.addOnPauseAction(_onPause = () => { gameObject.layer = LayerMask.NameToLayer("Ignore Raycast"); });
        GameManager.instance.addOnUnpauseAction(_onUnpause = () => { gameObject.layer = LayerMask.NameToLayer("UI"); });
    }

    private void OnDisable() {
        if (GameManager.isApplicationQuitting)
            return; // Jovo está em processo de encerramento e GameManager já foi destruído.
        GameManager.instance.removeOnPauseAction(_onPause);
        GameManager.instance.removeOnUpauseAction(_onUnpause);
    }

    public void fadeIn() {
        fadeIn(0.1f);
    }

    public void fadeIn(float step) {
        if (_cGroup.alpha >= _maxOpacity) return; // não é possível aumentar mais a opacidade
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);
        _fadeRoutine = StartCoroutine(Fade(true, step));
    }

    public void fadeOut() {
        fadeOut(0.1f);
    }

    public void fadeOut(float step) {
        if (_cGroup.alpha <= _minOpacity) return; // não é possível diminuir mais a opacidade
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);
        _fadeRoutine = StartCoroutine(Fade(false, step));
    }

    IEnumerator Fade(bool fadeIn, float step) {
        step = Mathf.Abs(step);
        int inOut = fadeIn ? 1 : -1;
        var c = _cGroup.alpha;
        while ((fadeIn && c <= _maxOpacity) || (c >= _minOpacity)) {
            c += inOut * step;
            _cGroup.alpha = Mathf.Clamp(c, _minOpacity, _maxOpacity);
            yield return new WaitForSeconds(0.3f);
        }

        //print("FadeEnd: " + _cGroup.alpha);
    }

}
