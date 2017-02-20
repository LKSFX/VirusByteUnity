using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Item {
    private void Awake() {
        _type = ItemType.BOMB;
    }

    public override void onDropStart() {
        base.onDropStart();
        _anim.SetBool("Grabbing", false);
    }
}
