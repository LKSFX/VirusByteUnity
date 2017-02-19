using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OffscreenDetector))]
public class Virus : MonoBehaviour {
    public bool isDebug = true;
    #region Propriedades
    public bool hasMoveController { // Retorna verdadeiro se este vírus tiver um controlador de movimento
        get { return _movementController != null; }
    }
    public bool isMoveAllowed { 
        //Retorna verdadeiro se este vírus tiver um controlador de movimento e estiver liberado para se mover
        get { return hasMoveController && _movementController.isMoveAllowed; }
    }
    public bool isMoving {
        // Retorna verdadeiro apenas quando o vírus tiver controlador de movimento, estiver liberado e tiver velocidade diferente de zero
        get { return isMoveAllowed && _movementController.speed != 0; }
    }
    #endregion

    protected Movement _movementController;
    protected virtual void Start() {
        OffscreenDetector detector = GetComponent<OffscreenDetector>();
        detector.onOut = onOffscreen;
        detector.onBottomOut = onBottomOut;
        detector.onTopOut = onTopOut;
        detector.onLeftOut = onLeftOut;
        detector.onRightOut = onRight;
    }

    public void setMove(bool move) {
        if (!hasMoveController) return; // não pode mover-se caso não haja controlador de movimento
        _movementController.setMove(move);
    }

    #region Vírus sai da tela
    /// <summary>
    /// Sinal chamado pelo OffscreenDetector quando este objeto sai da tela.
    /// </summary>
    public virtual void onOffscreen() {
        if (isDebug) {
            log("Offscreen()");
        }
        Destroy(gameObject);
    }

    public virtual void onBottomOut() {
        if (isDebug)
            log("Sai da tela por baixo.");
    }

    public virtual void onTopOut() {
        if (isDebug)
            log("Sai da tela por cima.");
    }

    public virtual void onLeftOut() {
        if (isDebug)
            log("Sai da tela pela esquerda.");
    }

    public virtual void onRight() {
        if (isDebug)
            log("Sai da tela pela direita.");
    }
    #endregion

    public void log(string msg) {
        print(gameObject.name + " -> " + msg);
    }
}
