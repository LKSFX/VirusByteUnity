using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OffscreenDetector))]
public class Virus : MonoBehaviour {
    public bool isDebug = true;

    protected virtual void Start() {
        OffscreenDetector detector = GetComponent<OffscreenDetector>();
        detector.onOut = onOffscreen;
        detector.onBottomOut = onBottomOut;
        detector.onTopOut = onTopOut;
        detector.onLeftOut = onLeftOut;
        detector.onRightOut = onRight;
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
