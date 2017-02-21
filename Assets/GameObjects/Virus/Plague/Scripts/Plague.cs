using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plague : Virus, IPointerClickHandler {
    // Use this for initialization
    protected override void Awake() {
        base.Awake();
	}

    public void OnPointerClick(PointerEventData eventData) {
        // O vírus plague é destruído por click
        _anim.SetTrigger("hurt");
        onDefeated();
        _isHit = true;
    }

}
