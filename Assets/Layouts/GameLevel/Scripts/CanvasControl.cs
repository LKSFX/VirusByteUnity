using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasControl : MonoBehaviour, IPauseAction {

    public GameObject _pauseMenu;

    private void Awake() {
        _pauseMenu = transform.Find("PauseMenu").gameObject;
    }

    // Use this for initialization
    void OnEnable() {
        GameManager.instance.addOnPauseAction(this);
        if (_pauseMenu != null) {
            if (_pauseMenu.activeInHierarchy) {
                GameManager.instance.setGamePause(true);
            }
        }
    }

    void OnDisable() {
        if (GameManager.isApplicationQuitting)
            return;
        GameManager.instance.removeOnPauseAction(this);
    }

    public void onPause() {
        _pauseMenu.SetActive(true);
    }

    public void onUnpause() {
        _pauseMenu.SetActive(false);
    }
}