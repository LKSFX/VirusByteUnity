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

    private List<IPauseAction> pauseActionList = new List<IPauseAction>();
    private Dictionary<Item.ItemType, ItemLevel> _itemLevelList;

    private void Awake() {
        load();
    }

    public void addCoins(int num) {
        _currentCoins += num;
    }

    private void load() {
        loadItemLevelList();
    }

    private void loadItemLevelList() {
        _itemLevelList = new Dictionary<Item.ItemType, ItemLevel>();
        _itemLevelList.Add(Item.ItemType.BOMB, new ItemLevel(0));
        _itemLevelList.Add(Item.ItemType.ANTIVIRUS, new ItemLevel(0));
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
            if (pauseActionList.Count > 0) {
                foreach (var action in pauseActionList) {
                    action.onPause();
                }
            }
        }
        else {
            // Jogo volta a escala de tempo anterior à pausa
            Time.timeScale = _gameTimeScale;
            Camera.main.GetComponent<Physics2DRaycaster>().eventMask = _rayMaskDefault;
            if (pauseActionList.Count > 0) {
                foreach (var action in pauseActionList) {
                    action.onUnpause();
                }
            }
        }
    }

    #region Get's
    public ItemLevel getItemLevel(Item.ItemType type) {
        return _itemLevelList[type];
    }
    #endregion

    #region Adições e remoções de ações tomadas durante estado de pausa
    public void addOnPauseAction(IPauseAction pauseAction) {
        if (pauseAction == null) return;// não aceita valor NULO
        pauseActionList.Add(pauseAction);
    }

    public void removeOnPauseAction(IPauseAction pauseAction) {
        pauseActionList.Remove(pauseAction);
    }

    #endregion

}
