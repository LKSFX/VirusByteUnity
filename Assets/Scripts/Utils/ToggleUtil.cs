using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ToggleUtil : MonoBehaviour {

    public Sprite toggleOn;
    public Sprite toggleOff;

    public void onValueChanged(bool state) {
        Image img = GetComponent<Image>();
        if (state && toggleOn != null) {
            img.sprite = toggleOn;
        }
        else if (toggleOff != null) {
            img.sprite = toggleOff;
        }
    }
}
