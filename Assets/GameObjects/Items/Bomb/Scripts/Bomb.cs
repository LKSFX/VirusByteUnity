using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Item {
    private void Awake() {
        _type = ItemType.BOMB;
    }

    public override void onDropStart() {
        _anim.SetBool("Grabbing", false);
        base.onDropStart();
    }

    public override void onDropHalfTargetScale() {
        Invoke("detonate", 0.5f);
        Invoke("explode", 1);
        // chamado método base após invokes para no caso desse item ser inserido no slot
        // os mesmos invokes poderem ser cancelados pela função onSlotEnter
        base.onDropHalfTargetScale();
    }

    public override void onSlotEnter() {
        base.onSlotEnter();
        CancelInvoke();
    }

    private void detonate() {
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        spr.enabled = false;
        if (effectActive != null) {
            Instantiate(effectActive, transform.position, Quaternion.identity);
        }
    }

    private void explode() {
        Collider2D[] collisionList = new Collider2D[20];
        int total = Physics2D.OverlapCircleNonAlloc(new Vector2(transform.position.x, transform.position.y), 3, collisionList);
        IExplosionDetector detector;
        for (int i = 0; i < total; i++) {
            detector = collisionList[i].gameObject.GetComponent<IExplosionDetector>();
            if (detector != null) {
                detector.onExplosionRange(_level);
            }
        }
        Destroy(gameObject);
    }
}
