using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plague : Virus, IPointerClickHandler {
    // Use this for initialization
    protected override void Awake() {
        base.Awake();
        gameObject.AddComponent<ColliderDraw>();
	}

    public void OnPointerClick(PointerEventData eventData) {
        // O vírus plague é destruído por click
        _anim.SetTrigger("hurt");
        Vibrator.vibrate(new long[] { 20, 20, 50, 20, 50, 10 }, -1);
        onDefeated();
        _isHit = true;
    }

}
