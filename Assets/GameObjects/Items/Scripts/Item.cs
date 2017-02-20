using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Item : ItemGrabber {
    public enum ItemType { ANTIVIRUS, BOMB }
    public GameObject icon;

    protected ItemType _type;
    protected Animator _anim;
    public ItemType type {
        get { return _type; }
    }

    protected override void Start() {
        base.Start();
        _anim = GetComponent<Animator>();
    }

    public override void onGrabStart() {
        base.onGrabStart();
        _anim.SetBool("Grabbing", true);
    }
}
