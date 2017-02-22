using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopButton : MonoBehaviour {

    public void switchTimeScale() {
        GameManager gm = GameManager.instance;
        gm.setGamePause(!gm.isGamePaused);
        if (gm.isGamePaused) {
            GetComponentInChildren<UnityEngine.UI.Text>().text = "Play";
        }
        else {
            GetComponentInChildren<UnityEngine.UI.Text>().text = "Pause";
        }
    }

}
