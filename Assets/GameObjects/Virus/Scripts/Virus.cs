using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OffscreenDetector), typeof(Animator))]
public class Virus : MonoBehaviour, IExplosionDetector, ILaserDetector {

    public bool isDebug = false;
    public GameObject effectSmoke; // Efeito para quando morrer carbonizado
    public GameObject effectHit; // Efeito para quando sofrer hit
    public GameObject effectExplode;
    public Material matElectrifiedDeath; 

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

    protected MovementController _movementController;
    protected SpriteRenderer _render;
    protected Animator _anim;
    protected bool _isHit;
    private bool _isAlive; // Vírus só causará danos quando cruzar o final da tela vivo

    //Hashcodes
    private int hashExploded = Animator.StringToHash("exploded");

    protected virtual void Awake() {
        _anim = GetComponent<Animator>();
        _isAlive = true;
        _movementController = GetComponent<MovementController>();
        _render = GetComponent<SpriteRenderer>();
        OffscreenDetector detector = GetComponent<OffscreenDetector>();
        detector.onOut = onOffscreen;
        detector.onBottomOut = onBottomOut;
        detector.onTopOut = onTopOut;
        detector.onLeftOut = onLeftOut;
        detector.onRightOut = onRight;
    }

    #region Efeitos tomados por contato com os efeitos dos itens
    /// <summary>
    /// Destruído por explosão de item bomba
    /// </summary>
    /// <param name="level"></param>
    public void onExplosionRange(ItemInfo level) {
        if (_anim.GetBool(hashExploded)) return; // objeto já está em estado de dano por explosão, retorna.
        _anim.SetTrigger("hurt");
        _anim.SetBool(hashExploded, true);
        onDefeated();
        if (effectExplode != null) // instancia efeito de transição, dano por explosão
            Instantiate(effectExplode, transform.position, Quaternion.identity);
        Invoke("turnRoasted", 0.4f);
        Invoke("onRoastedDeath", 2f);
    }

    private void turnRoasted() {
        _anim.SetTrigger("roasted");
    }

    public void onLaserRange(ItemInfo info) {
        _anim.SetBool("eletrified", true);
        onDefeated();
        if (info != null && info.effect != null) {
            LaserBeam lb = info.effect.GetComponent<LaserBeam>();
            if (lb != null) {
                transform.parent = info.effect.transform.parent;
            }
        }
        StartCoroutine(OnEletrifiedDeath());
    }

    #endregion


    #region Finalizações (fatalities)
    private void onRoastedDeath() {
        if (effectSmoke != null) // instancia efeito de fumaça
            Instantiate(effectSmoke, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator OnEletrifiedDeath() {

        yield return new WaitForSeconds(1.5f);

        _render.material = matElectrifiedDeath;

        yield return new WaitForSeconds(0.2f);

        _render.material.SetColor(Shader.PropertyToID("_Color"), Color.black);
        //_render.color = Color.black;

        yield return new WaitForSeconds(0.2f);

        Color c;
        float alpha = 1f;

        while (alpha > 0) {
            alpha -= Time.deltaTime * 2;
            c = _render.material.GetColor(Shader.PropertyToID("_Color"));
            c.a = alpha;
            _render.material.SetColor(Shader.PropertyToID("_Color"), c);
            yield return null;
        }

        Destroy(gameObject);
        // Fim da animação morte por eletricidade
    }

    #endregion

    /// <summary>
    /// Chamar este método quando o vírus for destruído e não mais representar uma ameaça.
    /// </summary>
    public virtual void onDefeated() {
        _isAlive = false;
        _render.sortingOrder = -10;
        setMove(false);
    }

    #region On Animation Events
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
