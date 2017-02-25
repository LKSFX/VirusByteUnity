using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasControl : MonoBehaviour, IPauseAction {

    // Use this for initialization
    void OnEnable() {
        GameManager.instance.addOnPauseAction(this);
    }

    void OnDisable() {
        if (GameManager.isApplicationQuitting)
            return;
        GameManager.instance.removeOnPauseAction(this);
    }

    public void onPause() {
        transform.Find("PauseMenu").gameObject.SetActive(true);
    }

    public void onUnpause() {
        transform.Find("PauseMenu").gameObject.SetActive(false);
    }
}