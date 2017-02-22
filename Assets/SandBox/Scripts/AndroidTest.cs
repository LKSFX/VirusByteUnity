using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AndroidTest : MonoBehaviour {

	public void toast(string msg) {
        Toast.show(msg);
	}
	
    public void restartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Toast.show("Level restarted!");
    }

    public void vibrate() {
        Vibrator.vibrate(new long[] {20,100,50,50,20,10,20,10}, -1);
    }
}
