using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OffscreenDetector), typeof(Animator))]
public class Virus : MonoBehaviour, IExplosionDetector {

    public bool isDebug = false;
    public GameObject effectSmoke; // Efeito para quando morrer carbonizado
    public GameObject effectHit; // Efeito para quando sofrer hit

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
    public bool isAlive {
        get { return _isAlive; }
    }
    #endregion

    protected Movement _movementController;
    protected Animator _anim;
    protected bool _isHit;
    private bool _isAlive; // Vírus só causará danos quando cruzar o final da tela vivo

    protected virtual void Awake() {
        _anim = GetComponent<Animator>();
        _isAlive = true;
        _movementController = GetComponent<Movement>();
        OffscreenDetector detector = GetComponent<OffscreenDetector>();
        detector.onOut = onOffscreen;
        detector.onBottomOut = onBottomOut;
        detector.onTopOut = onTopOut;
        detector.onLeftOut = onLeftOut;
        detector.onRightOut = onRight;
    }


    public void onExplosionRange(ItemLevel level) {
        _anim.SetTrigger("roasted");
        onDefeated();
        Invoke("onRoastedDeath", 2f);
    }

    public void onRoastedDeath() {
        if (effectSmoke != null)
            Instantiate(effectSmoke, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    /// <summary>
    /// Chamar este método quando o vírus for destruído e não mais representar uma ameaça.
    /// </summary>
    public virtual void onDefeated() {
        _isAlive = false;
        setMove(false);
    }

    #region Animation Events
    /// <summary>
    /// Chamado ao final da animação HURT
    /// </summary>
    public virtual void onAnimHurtEnd() {
        if (_isHit) {
            Destroy(gameObject, 0.2f);
        }
    }

    /// <summary>
    /// Chamado no meio da animação HURT
    /// </summary>
    public virtual void onAnimHurtHalf() {
        if (_isHit && effectHit != null) {
            Instantiate(effectHit, transform.position, Quaternion.identity);
        }
    }
    #endregion

    #region SET's
    public void setMove(bool move) {
        if (!hasMoveController) return; // não pode mover-se caso não haja controlador de movimento
        _movementController.setMove(move);
    }

    public void setSpeed(float speed) {
        if (!hasMoveController) return;
        _movementController.speed = speed;
    }
    #endregion

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
