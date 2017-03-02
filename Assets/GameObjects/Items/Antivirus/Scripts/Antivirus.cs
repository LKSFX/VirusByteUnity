using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antivirus : Item {

    private bool _allowDetonate;

	// Use this for initialization
	protected override void Awake () {
        base.Awake();
        _type = ItemType.ANTIVIRUS;
	}

    public override void onDropStart() {
        _anim.SetBool("Grabbing", false);
        base.onDropStart();
    }

    public override void onDropHalfTargetScale() {
        if (_allowDetonate) {
            //Invoke("detonate", 0.5f);
            //Invoke("explode", 1);
            setAllowGrab(false); // depois de ativado não se pode mais agarrá-lo
        }
        // chamado método base após invokes para no caso desse item ser inserido no slot
        // os mesmos invokes poderem ser cancelados pela função onSlotEnter
        base.onDropHalfTargetScale();
    }

    public override void onSlotEnter() {
        base.onSlotEnter();
        CancelInvoke();
    }
}
