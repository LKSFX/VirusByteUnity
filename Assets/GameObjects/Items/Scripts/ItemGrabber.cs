using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrabber : Grabber {

    private SpriteRenderer _sprRender;

    protected override void Start() {
        base.Start();
        _sprRender = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update() {
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

    public override void onDropHalfTargetScale() {
        base.onDropHalfTargetScale();
        if (_sprRender) {
            _sprRender.sortingLayerName = "LayerSprites0";
        }
        GameManager.instance.addCoins(1);
    }

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
}
