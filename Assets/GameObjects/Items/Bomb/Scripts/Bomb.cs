using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public GameObject bombBody;
    private GameObject _bomb;

	void Awake () {
        _bomb = Instantiate(bombBody, transform, false);
        _bomb.GetComponent<Grabber>().isRelative = true;
        _bomb.GetComponent<Animator>().enabled = true;
        print("AWAKE");
	}
	
	
}
