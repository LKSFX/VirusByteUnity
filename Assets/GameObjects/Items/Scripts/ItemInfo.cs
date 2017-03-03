using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo {
    private int _level;
    public Item item;
    public GameObject effect;

    public int level {
        get { return _level; }
    }

    public ItemInfo(ItemInfo itemInfo) {
        _level = itemInfo._level;
        item = itemInfo.item;
        effect = itemInfo.effect;
    }

    public ItemInfo(int level) {
        _level = level;
    }
}
