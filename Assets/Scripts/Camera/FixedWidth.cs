using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedWidth : MonoBehaviour {
    readonly float fixedWidth = 480;
	// Use this for initialization
	void Start () {
        var camera = GetComponent<Camera>();
        // Exemplo, no caso de uma resolução de 720x1280 a proporção seria de
        // 1.78 pixel de altura para cada pixel de largura 
        var ratio = (float)Screen.height / (float)Screen.width;
        var screenHeight = fixedWidth * ratio;
        var size = screenHeight / 160;
        camera.orthographicSize = size;
    }
	
}
