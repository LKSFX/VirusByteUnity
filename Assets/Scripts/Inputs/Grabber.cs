using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Os movimentos vão mover o transform parent do objeto onde estiver inserido este script.
/// Logo, se faz necessário o objeto em questão estar dentro de outro objeto, que servirá de eixo.
/// Isso evita problemas, no caso deste objeto onde o script se encontra estar animado. 
/// Em geral, deve estar ligado a um sprite, algo visível.
/// </summary>
public class Grabber : MonoBehaviour {

    private Transform _prTransform; // parent transform
    private Camera _camera;
    private Vector3 _originalScale;
    private Vector3 _metaScale;
    private Vector3 _halfScale;

    private GrabState _state = GrabState.STATIC;

    private bool _isDebug = true;
    private bool _isGrabbed;
    private bool _hasParent;

    public enum GrabState { GROWING, STATIC, SHRINKING }

    public bool isRelative = false;
    [Range(1, 5)]
    public float targetGrabScale = 1f; // escala do objeto quando agarrado
    [Range(0.1f, 1f)]
    public float growDuration;
    [Range(0.1f, 1f)]
    public float shrinkDuration;

    protected virtual void Start() {
        _prTransform = transform.parent;
        _hasParent = _prTransform != null;
        _camera = FindObjectOfType<Camera>();
        _originalScale = transform.localScale;
        var vec = Vector3.one * targetGrabScale;
        vec.z = 0; // Não modificar o eixo Z
        _metaScale = _originalScale + vec;
        _halfScale = _originalScale + vec * .5f; // Half meta convertido
    }

    private void OnMouseDrag() {
        // Arrasta objeto 
        if (isRelative && _hasParent) {
            // Posição será ajustada em relação ao objeto PAI
            var prPos = _prTransform.position; // posição do objeto pai
            var localPos = transform.localPosition; // posição local atual
            var mousePos = Input.mousePosition; // posição atual do mouse ou touch
            var mousePosWorld = _camera.ScreenToWorldPoint(mousePos); // posição do mouse em relação ao mundo
            localPos.z = 0;
            var tPos = mousePosWorld - localPos;
            _prTransform.position = tPos;
        } else {
            // Posição será ajustada em relação ao MUNDO
            var mousePos = Input.mousePosition;
            var mousePosWorld = _camera.ScreenToWorldPoint(mousePos);
            mousePosWorld.z = 0;
            transform.position = mousePosWorld;
        }
        grabStart();
    }

    private void OnMouseUp() {
        dropStart();
    }

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

    // GETS públicos

    public GrabState getGrabState() {
        return _state;
    }

    // Métodos privados não implementáveis externamente

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

    // Métodos públicos implementáveis externamente

    public virtual void onGrabStart() {
        if (_isDebug)
            log("grabStart");
    }

    public virtual void onGrabHalfTargetScale() {
        if (_isDebug) {
            log("growHalf");
        }
    }

    public virtual void onGrabTargetScale() {
        if (_isDebug)
            log("growEnd");
    }

    public virtual void onDropStart() {
        if (_isDebug)
            log("dropStart");
    }

    public virtual void onDropHalfTargetScale() {
        if (_isDebug)
            log("dropHalf");
    }

    public virtual void onDropTargetScale() {
        if (_isDebug)
            log("shrinkEnd");
    }

    public void log(string msg) {
        var objectName = isRelative && _hasParent ? transform.parent.name : gameObject.name;
        print(objectName + " -> " + msg);
    }
}
