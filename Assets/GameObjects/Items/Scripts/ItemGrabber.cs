using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemGrabber : Grabber {

    private SpriteRenderer _sprRender;
    private ItemCanvas _canvas;
    private Slot _slot;
    /// <summary>
    /// Se este Item estiver dentro de um objeto PARENT,
    /// então esta referência deverá indicar o transform do objeto PAI,
    /// caso contrário, deve indicar seu próprio transform.
    /// </summary>
    protected Transform _relativeTransform;

    protected override void Start() {
        base.Start();
        _sprRender = GetComponent<SpriteRenderer>();
        _canvas = FindObjectOfType<ItemCanvas>();
        _relativeTransform = transform;
    }

    protected override void Update() {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space)) {
            print("Pressed Space key");
            StartCoroutine(Fade());
        }
    }

    public override void onGrabHalfTargetScale() {
        base.onGrabHalfTargetScale();
        if (_sprRender != null) {
            _sprRender.sortingLayerName = "LayerItems";
        }
    }

    public override void onDropStart() {
        base.onDropStart();
        if (_canvas != null) {
            // No canvas de itens, este objeto acompanhará a camera
            _relativeTransform.parent = _canvas.transform;
        }
    }

    public override void onDropHalfTargetScale() {
        base.onDropHalfTargetScale();
        if (_sprRender) {
            _sprRender.sortingLayerName = "LayerSprites0";
        }
        GameManager.instance.addCoins(1);
        checkDropForSlot();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //print(gameObject.name + " Collided with " +collision.gameObject.name);
        _slot = collision.gameObject.GetComponent<Slot>();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        //print(gameObject.name + " Collision exit " + collision.gameObject.name);
        _slot = null;
    }

    private void checkDropForSlot() {
        if (_slot != null) {
            if (_slot.checkItemDrop(gameObject)) {
                //Item está no slot agora
                onSlotEnter();
            }
        }
    }

    // TESTE
    IEnumerator Fade() {
        var c = _sprRender.color;
        var f = c.a;
        while (f >= 0) {
            f -= .1f;
            c.a = f;
            _sprRender.color = c;
            yield return new WaitForSeconds(0.3f);
        }

        print("FadeEnd: " + _sprRender.color);
    }

    #region Métodos implementáveis
    public virtual void onSlotEnter() {
        setMetaScale();
    }
    #endregion

}
