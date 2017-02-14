using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public sealed class RangeEx : PropertyAttribute {
    public readonly float min;
    public readonly float max;
    public readonly float step;

    public RangeEx(float min, float max, float step) {
        this.min = min;
        this.max = max;
        this.step = step;
    }
	
}
