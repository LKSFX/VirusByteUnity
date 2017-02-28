using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Os movimentos vão mover o transform parent do objeto onde estiver inserido este script.
/// Logo, se faz necessário o objeto em questão estar dentro de outro objeto, que servirá de eixo.
/// Isso evita problemas, no caso deste objeto onde o script se encontra estar animado. 
/// Em geral, deve estar ligado a um sprite, algo visível.
/// </summary>
public class Grabber : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler {

    private Transform _prTransform; // parent transform
    private Vector3 _originalScale;
    private Vector3 _metaScale;
    private Vector3 _halfScale;
    private Vector3 _mouseDeltaPos;
    private Vector3 _pressDisplacement;
    protected Camera _camera;

    private GrabState _state = GrabState.STATIC;

    public bool isDebug = false;
    private bool _allowGrab;
    private bool _hasParent;
    protected bool _isGrabbed;

    public enum GrabState { GROWING, STATIC, SHRINKING }

    public bool isRelative = false;
    [Range(1, 5)]
    public float targetGrabScale = 1f; // escala do objeto quando agarrado
    [Range(0.1f, 1f)]
    public float growDuration;
    [Range(0.1f, 1f)]
    public float shrinkDuration;

    protected virtual void Awake() {
        _prTransform = transform.parent;
        _hasParent = _prTransform != null;
        _camera = FindObjectOfType<Camera>();
        _originalScale = transform.localScale;
        var vec = Vector3.one * targetGrabScale;
        vec.z = 0; // Não modificar o eixo Z
        _metaScale = _originalScale + vec;
        _halfScale = _originalScale + vec * .5f; // Half meta convertido
    }

    protected virtual void Update() {
        if (_isGrabbed) {
            positionUpdate(Input.mousePosition);
        }
    }

    private void positionUpdate(Vector3 mousePos) {
        // Arrasta objeto 
        if (isRelative && _hasParent) {
            // Posição será ajustada em relação ao objeto PAI
            var prPos = _prTransform.position; // posição do objeto pai
            var localPos = transform.localPosition; // posição local atual
            var mousePosWorld = _camera.ScreenToWorldPoint(mousePos); // posição do mouse em relação ao mundo
            localPos.z = 0;
            var tPos = mousePosWorld - localPos;
            _prTransform.position = tPos;
        }
        else {
            // Posição será ajustada em relação ao MUNDO
            var mousePosWorld = _camera.ScreenToWorldPoint(mousePos);
            mousePosWorld.z = transform.position.z;
            transform.position = mousePosWorld - _pressDisplacement;
        }
        _mouseDeltaPos = mousePos;
    }

    public void OnDrag(PointerEventData eventData) {
        //positionUpdate(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData) {
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (!_allowGrab) return; // não pode agarrar
        if (isDebug)
            print("DragBegin");
        grabStart();
        _mouseDeltaPos = Input.mousePosition;
        var localPressPos = _camera.ScreenToWorldPoint(_mouseDeltaPos);
        localPressPos.z = 0;
        _pressDisplacement = localPressPos - transform.position;
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (isDebug)
            print("DragEnd");
        if (_isGrabbed)
            dropStart();
    }

    //private void OnMouseUp() {
    //    if (_isGrabbed) {
    //        dropStart();
    //    }
    //}

    IEnumerator Grow() {
        bool reachedHalfScale = false;
        float startTime = Time.time;
        Vector3 deltaScale = transform.localScale;
        Vector3 currentScale = Vector3.Lerp(deltaScale, _metaScale, 0);
        while (currentScale.sqrMagnitude < _metaScale.sqrMagnitude) {
            // Quando estiver sendo agarrado
            var timeCovered = (Time.time - startTime);
            var fracScale = timeCovered / growDuration;
            currentScale = Vector3.Lerp(deltaScale, _metaScale, fracScale);
            transform.localScale = currentScale;
            if (!reachedHalfScale && currentScale.sqrMagnitude >= _halfScale.sqrMagnitude) {
                onGrabHalfTargetScale();
                reachedHalfScale = true; // Este pedaço do código será chamado apenas uma vez durante o estado GROW
            }
            yield return null;
        }
        // Evita que o sinal de meia escala, que pode conter chamadas importantes, passe em branco
        if (!reachedHalfScale) {
            onGrabHalfTargetScale();
        }
        growEnd();
    }

    IEnumerator Shrink() {
        bool reachedHalfScale = false;
        float startTime = Time.time;
        Vector3 deltaScale = transform.localScale;
        Vector3 currentScale = Vector3.Lerp(deltaScale, _originalScale, 0);
        while(currentScale.sqrMagnitude > _originalScale.sqrMagnitude) {
            // Quando estiver caindo
            var timeCovered = (Time.time - startTime);
            var fracScale = timeCovered / shrinkDuration;
            currentScale = Vector3.Lerp(deltaScale, _originalScale, fracScale);
            transform.localScale = currentScale;
            if (!reachedHalfScale && currentScale.sqrMagnitude <= _halfScale.sqrMagnitude) {
                onDropHalfTargetScale();
                reachedHalfScale = true; // Este pedaço do código será chamado apenas uma vez durante o estado SHRINK
            }
            yield return null;
        }
        shrinkEnd();
    }

    #region SET's públicos
    public void setMetaScale() {
        transform.localScale = _metaScale;
    }
    public void setOriginalScale() {
        transform.localScale = _originalScale;
    }
    public void setAllowGrab(bool allow) {
        _allowGrab = allow;
    }
    #endregion

    #region GET's públicos

    public GrabState getGrabState() {
        return _state;
    }
    #endregion

    #region Métodos privados não implementáveis externamente

    private void grabStart() {
        if (!_isGrabbed) {
            // Grab só iniciará uma vez
            _isGrabbed = true;
            _state = GrabState.GROWING;
            StopAllCoroutines();
            StartCoroutine(Grow());
            onGrabStart(); // Chama método aviso Grab para as subclasses poderem implementar
        }
    }

    private void growEnd() {
        _state = GrabState.STATIC;
        onGrabTargetScale(); // Chama método implementável quando a animação de GROW tiver terminado
    }

    private void dropStart() {
        _isGrabbed = false;
        _state = GrabState.SHRINKING;
        StopAllCoroutines();
        StartCoroutine(Shrink());
        onDropStart(); // Chama método aviso Drop para as subclasses poderem implementar
    }

    private void shrinkEnd() {
        _state = GrabState.STATIC;
        onDropTargetScale(); // Chama método implementável quando a animação SHRINK tiver terminado
    }
    #endregion

    #region Métodos públicos implementáveis externamente

    public virtual void onGrabStart() {
        if (isDebug)
            log("grabStart");
    }

    public virtual void onGrabHalfTargetScale() {
        if (isDebug) {
            log("growHalf");
        }
    }

    public virtual void onGrabTargetScale() {
        if (isDebug)
            log("growEnd");
    }

    public virtual void onDropStart() {
        if (isDebug)
            log("dropStart");
    }

    public virtual void onDropHalfTargetScale() {
        if (isDebug)
            log("dropHalf");
    }

    public virtual void onDropTargetScale() {
        if (isDebug)
            log("shrinkEnd");
    }
    #endregion

    public void log(string msg) {
        var objectName = isRelative && _hasParent ? transform.parent.name : gameObject.name;
        print(objectName + " -> " + msg);
    }

}
