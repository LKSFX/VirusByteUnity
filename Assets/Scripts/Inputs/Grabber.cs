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
    private Vector3 _deltaScale;
    private Vector3 _halfScale;

    private GrabState _state = GrabState.STATIC;

    private bool _isDebug = true;
    private bool _isGrabbed;
    private bool _hasCalledHalfAlert; //definido como falso quando agarrado e solto; verdadeiro quando a escala ultrapassa a metade da meta. 

    private float _startTime;

    public enum GrabState { GROWING, STATIC, SHRINKING }

    [Range(1, 5)]
    public float targetGrabScale = 1f; // escala do objeto quando agarrado
    [Range(0.1f, 1f)]
    public float growDuration;
    [Range(0.1f, 1f)]
    public float shrinkDuration;

    private void Start() {
        _prTransform = transform.parent;
        _camera = FindObjectOfType<Camera>();
        _originalScale = transform.localScale;
        var vec = Vector3.one * targetGrabScale;
        vec.z = 0; // Não modificar o eixo Z
        _metaScale = _originalScale + vec;
        _halfScale = _originalScale + vec * .5f; // Half meta convertido
    }

    private void OnMouseDrag() {
        // Arrasta objeto 
        var prPos = _prTransform.position; // posição do objeto pai
        var localPos = transform.localPosition; // posição local atual
        var mousePos = Input.mousePosition; // posição atual do mouse ou touch
        var mousePosWorld = _camera.ScreenToWorldPoint(mousePos); // posição do mouse em relação ao mundo
        localPos.z = 0;
        var tPos = mousePosWorld - localPos;
        _prTransform.position = tPos;
        grabStart();
    }

    private void OnMouseUp() {
        dropStart();
    }

    private void Update() {
        float timeCovered;
        float fracScale;
        Vector3 currentScale;
        switch (_state) {
            case GrabState.GROWING:
                // Quando estiver sendo agarrado
                timeCovered = (Time.time - _startTime);
                fracScale = timeCovered / growDuration;
                currentScale = Vector3.Lerp(_deltaScale, _metaScale, fracScale);
                transform.localScale = currentScale;
                if (currentScale.sqrMagnitude >= _metaScale.sqrMagnitude) {
                    growEnd();
                }
                else if (!_hasCalledHalfAlert && currentScale.sqrMagnitude >= _halfScale.sqrMagnitude) {
                    onGrabHalfTargetScale();
                    _hasCalledHalfAlert = true; // Este pedaço do código será chamado apenas uma vez durante o estado GROW
                }
                break;
            case GrabState.SHRINKING:
                // Quando estiver caindo
                timeCovered = (Time.time - _startTime);
                fracScale = timeCovered / shrinkDuration;
                currentScale = Vector3.Lerp(_deltaScale, _originalScale, fracScale);
                transform.localScale = currentScale;
                if (currentScale.sqrMagnitude <= _originalScale.sqrMagnitude) {
                    shrinkEnd();
                }
                else if (!_hasCalledHalfAlert && currentScale.sqrMagnitude <= _halfScale.sqrMagnitude) {
                    onDropHalfTargetScale();
                    _hasCalledHalfAlert = true; // Este pedaço do código será chamado apenas uma vez durante o estado SHRINK
                }
                break;
            case GrabState.STATIC:

                break;
        }
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
            _startTime = Time.time;
            _hasCalledHalfAlert = false;
            _deltaScale = transform.localScale;
            
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
        _startTime = Time.time;
        _hasCalledHalfAlert = false;
        _deltaScale = transform.localScale;
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
        print(transform.parent.name + " -> " + msg);
    }
}
