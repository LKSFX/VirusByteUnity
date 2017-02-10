using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour {

    public void switchTimeScale() {
        if (Time.timeScale == 1) {
            Time.timeScale = 0;
            GetComponentInChildren<UnityEngine.UI.Text>().text = "Play";
        }
        else {
            Time.timeScale = 1;
            GetComponentInChildren<UnityEngine.UI.Text>().text = "Stop";
        }
    }

}
