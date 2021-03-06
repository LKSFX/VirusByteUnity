﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopButton : MonoBehaviour, IPauseAction {

    public void switchTimeScale() {
        GameManager gm = GameManager.instance;
        gm.setGamePause(!gm.isGamePaused);
    }

    private void OnEnable() {
        GameManager.instance.addOnPauseAction(this);
    }

    private void OnDisable() {
        if (!GameManager.isApplicationQuitting) {
            GameManager.instance.removeOnPauseAction(this);
        }
    }

    public void onPause() {
        gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = "Play";
    }

    public void onUnpause() {
        gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = "Pause";
    }
}
