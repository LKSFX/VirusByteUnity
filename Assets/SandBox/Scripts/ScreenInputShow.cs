using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenInputShow : MonoBehaviour {
    Text _text;
    Camera _cam;
    // Use this for initialization
    void Start () {
        _text = GetComponent<Text>();
        _cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        _text.text = "HorEx: " + (_cam.orthographicSize * Screen.width / Screen.height)
            + "\nTouch: " + _cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0))
            + "\n ScreenWidth: " + Screen.width + " | ScreenHeight: " + Screen.height
            + "\n w/h: " + ((float)Screen.width/Screen.height);
	}
}
