using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

    private Camera _camera;

	void Start () {
        _camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        var pos = _camera.transform.position;
        pos.z = 0;
        transform.position = pos;
	}
}
