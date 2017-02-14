using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GenericSingleton<GameManager> {

    private int _currentCoins;

    public void addCoins(int num) {
        _currentCoins += num;
    }
}
