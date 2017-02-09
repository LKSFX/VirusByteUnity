using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsViewer : MonoBehaviour {
    float deltaTime = 0f;
    GUIStyle style = new GUIStyle();
    Rect rect;

	// Use this for initialization
	void Start () {
        rect = new Rect(0, 0, Screen.width, Screen.height * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = Screen.height * 2 / 100;
        style.normal.textColor = new Color(1f, 1f, 0f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}

    private void OnGUI() {
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}
