
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraLevel : MonoBehaviour {
    private bool _isMoveAllow = true;
    [Header("Velocidade da câmera.")]
    [Tooltip("Define a velocidade inicial da câmera.")]
    [Range(-10f,10f)]
    public float speed = 0;

	// Use this for initialization
	void Awake () {
        Vibrator.vibrate(new long[] { 20, 5 }, -1);
    }
	
	// Update is called once per frame
	void Update () {
		if (_isMoveAllow) {
            transform.position = transform.position + Vector3.up * speed * Time.deltaTime;
        }
	}

    void onMoveCheckPosition() { }
}
