using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Item {
    private void Awake() {
        _type = ItemType.BOMB;
    }
}
