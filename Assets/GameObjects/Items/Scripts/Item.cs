using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Item : ItemGrabber {
    public enum ItemType { ANTIVIRUS, BOMB }
    public GameObject icon;
    public GameObject effectActive;

    protected ItemType _type;
    protected ItemInfo _info;
    protected Animator _anim;
    public ItemType type {
        get { return _type; }
    }

    protected override void Awake() {
        base.Awake();
        _anim = GetComponent<Animator>();
        _info = GameManager.instance.getItemLevel(_type);
        setAllowGrab(true);
    }

    public override void onGrabStart() {
        base.onGrabStart();
        _anim.SetBool("Grabbing", true);
    }
}
