using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffscreenDetector : MonoBehaviour {
    public delegate void Offscreen();

    private Camera _cam;
    public Offscreen onOut;
    public Offscreen onTopOut;
    public Offscreen onLeftOut;
    public Offscreen onRightOut;
    public Offscreen onBottomOut;

    private void Start() {
        _cam = Camera.main;
    }

    private void OnBecameInvisible() {
        if (_cam == null) return; // Evita camera NULL enquanto no Editor
        Vector3 bottomLeft = _cam.ScreenToWorldPoint(new Vector3(0, 0));
        Vector3 topRight = _cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        Vector3 currentPos = transform.position;

        if (currentPos.y < bottomLeft.y) {
            // Saiu por baixo
            if (onOut != null)
                onOut();
            if (onBottomOut != null)
                onBottomOut();
        }
        else if (currentPos.y > topRight.y) {
            // Saiu por cima
            if (onOut != null)
                onOut();
            if (onTopOut != null)
                onTopOut();
        }
        else if (currentPos.x < bottomLeft.x) {
            // Saiu pela esquerda
            if (onOut != null)
                onOut();
            if (onLeftOut != null)
                onLeftOut();
        }
        else if (currentPos.x > topRight.x) {
            // Saiu pela direita
            if (onOut != null)
                onOut();
            if (onRightOut != null)
                onRightOut();
        }
    }
}
