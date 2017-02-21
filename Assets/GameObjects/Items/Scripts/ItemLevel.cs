using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLevel {
    private int _level;

    public int level {
        get { return _level; }
    }

    public ItemLevel(int level) {
        _level = level;
    }
}
