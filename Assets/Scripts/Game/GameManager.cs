using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private int _currentCoins;

    public static GameManager instance;

    private void Awake() {
        instance = this;
    }

    public void addCoins(int num) {
        _currentCoins += num;
    }
}
