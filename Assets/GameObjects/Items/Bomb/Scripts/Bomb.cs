using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public GameObject bombBody;
    private GameObject _bomb;

	void Awake () {
        _bomb = (GameObject)Instantiate(bombBody, transform);
        print("AWAKE");
	}
	
	
}
