using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsViewer : MonoBehaviour {
    private GUIStyle _style = new GUIStyle();
    private Rect _rect;
    private Color _textColor;
    private string _text;

    public float updateInterval = 0.5f;

    private float _accum = 0; //FPS acumulados durante o intervalo
    private int _frames = 0; // Quadros desenhados durante o intervalo
    private float _timeleft; // Tempo restante para o invervalo atual

	// Use this for initialization
	void Start () {
        _rect = new Rect(0, 0, Screen.width, Screen.height * 2 / 100);
        _style.alignment = TextAnchor.UpperLeft;
        _style.fontSize = Screen.height * 2 / 100;
        _style.normal.textColor = new Color(1f, 1f, 0f, 1f);
        _timeleft = updateInterval;
	}
	
	// Update is called once per frame
	void Update () {
        _timeleft -= Time.deltaTime;
        _accum += Time.timeScale / Time.deltaTime;
        ++_frames;

        // Intervalo terminou — atualiza o texto da GUI e recomeça o intervalo
        if (_timeleft <= 0) {
            // Mostra dois dígitos fracionários
            float fps = _accum / _frames;
            _text = string.Format("{0:F2} FPS", fps);
            if (fps < 30) {
                _style.normal.textColor = Color.red;
            } else {
                _style.normal.textColor = Color.green;
            }
            _timeleft = updateInterval;
            _accum = 0f;
            _frames = 0;
        }
	}

    private void OnGUI() {
        // Desenha os quadros por segundo na tela
        GUI.Label(_rect, _text, _style);
    }
}
