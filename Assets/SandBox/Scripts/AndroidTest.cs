using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidTest : MonoBehaviour {

	public void toast(string msg) {
        Toast.show(msg);
	}
	
    public void vibrate() {
        Vibrator.vibrate(new long[] {20,100,50,50,20,10,20,10}, -1);
    }
}
