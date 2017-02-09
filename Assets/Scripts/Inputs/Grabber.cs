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

    private GrabState _state = GrabState.STATIC;

    private bool _isDebug = true;
    private bool _isGrabbed;
    private bool _hasCalledHalfAlert; //definido como falso quando agarrado e solto; verdadeiro quando a escala ultrapassa a metade da meta. 

    private float _startTime;
    private float _scaleLength;

    public enum GrabState { GROWING, STATIC, SHRINKING }

    [Range(1, 5)]
    public float targetGrabScale = 1f; // escala do objeto quando agarrado
    [Range(0.1f, 1f)]
    public float growSpeed;
    [Range(0.1f, 1f)]
    public float shrinkSpeed;

    private void Start() {
        _prTransform = transform.parent;
        _camera = FindObjectOfType<Camera>();
        _originalScale = transform.localScale;
        var vec = Vector3.one * targetGrabScale;
        vec.z = 0; // Não modificar o eixo Z
        _metaScale = _originalScale + vec;
        _scaleLength = Vector3.Distance(_originalScale, _metaScale);    
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
        float sclCovered;
        float fracScale;
        float sclTargetDistance;
        Vector3 currentScale;
        switch (_state) {
            case GrabState.GROWING:
                // Quando estiver sendo agarrado
                sclCovered = (Time.time - _startTime) * growSpeed;
                fracScale = sclCovered / _scaleLength;
                currentScale = Vector3.LerpUnclamped(transform.localScale, _metaScale, fracScale);
                transform.localScale = currentScale;
                sclTargetDistance = Mathf.Abs(currentScale.sqrMagnitude - _metaScale.sqrMagnitude);
                if (sclTargetDistance < 0.01) {
                    transform.localScale = _metaScale;
                    growEnd();
                }
                break;
            case GrabState.SHRINKING:
                // Quando estiver caindo
                sclCovered = (Time.time - _startTime) * shrinkSpeed;
                fracScale = sclCovered / _scaleLength;
                currentScale = Vector3.LerpUnclamped(transform.localScale, _originalScale, fracScale);
                transform.localScale = currentScale;
                sclTargetDistance = Mathf.Abs(currentScale.sqrMagnitude - _originalScale.sqrMagnitude);
                if (sclTargetDistance < 0.01) {
                    transform.localScale = _originalScale;
                    shrinkEnd();
                }
                break;
            case GrabState.STATIC:

                break;
        }
    }

    // Métodos privados não implementáveis

    private void grabStart() {
        if (!_isGrabbed) {
            // Grab só iniciará uma vez
            if (_isDebug)
                log("grabStart");
            _isGrabbed = true;
            _state = GrabState.GROWING;
            _startTime = Time.time;
            _hasCalledHalfAlert = false;
        }
    }

    private void growEnd() {
        if (_isDebug)
            log("growEnd");
        _state = GrabState.STATIC;
    }

    private void dropStart() {
        if (_isDebug)
            log("dropStart");
        _isGrabbed = false;
        _state = GrabState.SHRINKING;
        _startTime = Time.time;
        _hasCalledHalfAlert = false;
    }

    private void shrinkEnd() {
        if (_isDebug)
            log("shrinkEnd");
        _state = GrabState.STATIC;
    }

    // Métodos públicos implementáveis

    public virtual void onGrabStart() { }

    public virtual void onGrabHalfTargetScale() { }

    public virtual void onGrabTargetScale() { }

    public virtual void onDropStart() { }

    public virtual void onDropHalfTargetScale() { }

    public virtual void onDropTargetScale() { }

    public void log(string msg) {
        print(transform.parent.name + " -> " + msg);
    }
}
