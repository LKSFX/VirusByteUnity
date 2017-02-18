using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : GenericSingleton<GameManager> {

    public bool isGamePaused {
        get { return _isGamePaused; }
    }

    public const int DEF_LAYER = 0;
    public const int UI_LAYER = 5;

    private int _currentCoins;
    private int _rayMaskOnPause = 1 << UI_LAYER;
    private int _rayMaskDefault = 1 << DEF_LAYER | 1 << UI_LAYER;
    private float _gameTimeScale = 1;
    private bool _isGamePaused;

    public delegate void Action();
    private Action _onPause;
    private Action _onUnpause;

    public void addCoins(int num) {
        _currentCoins += num;
    }

    /// <summary>
    /// Pausa ou despausa o jogo
    /// </summary>
    /// <param name="pause"></param>
    public void setGamePause(bool pause) {
        _isGamePaused = pause;
        Camera cam = Camera.main;
        if (pause) {
            // Jogo em pausa
            _gameTimeScale = Time.timeScale;
            Time.timeScale = 0;
            Camera.main.GetComponent<Physics2DRaycaster>().eventMask = _rayMaskOnPause;
            if (_onPause != null) {
                _onPause();
            }
        }
        else {
            // Jogo volta a escala de tempo anterior à pausa
            Time.timeScale = _gameTimeScale;
            Camera.main.GetComponent<Physics2DRaycaster>().eventMask = _rayMaskDefault;
            if (_onUnpause != null) {
                _onUnpause();
            }
        }
    }

    #region Adições e remoções de ações tomadas durante estado de pausa
    public void addOnPauseAction(Action action) {
        _onPause += action;
    }

    public void removeOnPauseAction(Action action) {
        _onPause -= action;
    }

    public void addOnUnpauseAction(Action action) {
        _onUnpause += action;
    }

    public void removeOnUpauseAction(Action action) {
        _onUnpause -= action;
    }
    #endregion
}
