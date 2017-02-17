using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemGrabber))]
public class Item : MonoBehaviour {
    public enum ItemType { ANTIVIRUS, BOMB }
    public GameObject icon;
    protected ItemType _type;
    public ItemType type {
        get { return _type; }
    }
}
