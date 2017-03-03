using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour, IItemEffect {

	private bool _isActive;
    private ItemInfo _itemInfo;

    private void Start() {
        _isActive = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (_isActive) {
            //Enquanto ativo
            GameObject go = collision.gameObject;
            ILaserDetector ld = go.GetComponent<ILaserDetector>();
            if (ld != null)
                ld.onLaserRange(_itemInfo);
        }
    }

    public void setActive(bool active) {
        _isActive = active;
    }

    public void setItemInfo(ItemInfo itemInfo) {
        _itemInfo = new ItemInfo(itemInfo);
        _itemInfo.effect = this.gameObject;
    }

    public void finishedAnimation() {
        setActive(false);
    }
}
