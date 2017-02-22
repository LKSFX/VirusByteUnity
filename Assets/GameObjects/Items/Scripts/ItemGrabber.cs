using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemGrabber : Grabber {

    
    private Slot _slotHovered;
    private SpriteRenderer _sprRender;
    private CameraFollower _canvas;
    /// <summary>
    /// Se este Item estiver dentro de um objeto PARENT,
    /// então esta referência deverá indicar o transform do objeto PAI,
    /// caso contrário, deve indicar seu próprio transform.
    /// </summary>
    protected Transform _relativeTransform;

    protected override void Awake() {
        base.Awake();
        _sprRender = GetComponent<SpriteRenderer>();
        _canvas = FindObjectOfType<CameraFollower>();
        _relativeTransform = transform;
    }

    public override void onGrabHalfTargetScale() {
        base.onGrabHalfTargetScale();
        if (_sprRender != null) {
            _sprRender.sortingLayerName = "LayerItems";
        }
        gameObject.layer = LayerMask.NameToLayer("UI");
    }

    public override void onDropStart() {
        base.onDropStart();
        if (_canvas != null) {
            // No canvas de itens, este objeto acompanhará a camera
            _relativeTransform.parent = _canvas.transform;
        }
        checkDropForSlot();
    }

    public override void onDropHalfTargetScale() {
        base.onDropHalfTargetScale();
        if (_sprRender) {
            _sprRender.sortingLayerName = "LayerSprites0";
        }
        gameObject.layer = LayerMask.NameToLayer("Default");
        GameManager.instance.addCoins(1);
        
    }

    private void OnTriggerStay2D(Collider2D collision) {
        //print("collision width: " + collision.gameObject);
        Slot slot = collision.gameObject.GetComponent<Slot>();
        if (slot != null) {
            slot.fadeIn(1f);
            if (_slotHovered != null && slot != _slotHovered)
                _slotHovered.fadeOut(1f);
        }
        _slotHovered = slot;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        print(gameObject.name + " Collision exit " + collision.gameObject.name);
        Slot slot = collision.gameObject.GetComponent<Slot>();
        if (_slotHovered != null && _slotHovered == slot) {
            _slotHovered.fadeOut(1f);
            _slotHovered = null;
        }
    }

    private void checkDropForSlot() {
        if (_slotHovered != null) {
            _slotHovered.checkItemDrop(gameObject);
        }
    }

    #region Métodos implementáveis
    public virtual void onSlotEnter() {
        setMetaScale();
    }
    #endregion

}
