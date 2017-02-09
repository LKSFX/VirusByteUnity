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

    private void Start() {
        _prTransform = transform.parent;
        _camera = FindObjectOfType<Camera>();
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
    }

}
