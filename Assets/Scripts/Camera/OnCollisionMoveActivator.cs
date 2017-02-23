using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionMoveActivator : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        IOnCollisionMove moveController = collision.gameObject.GetComponent<IOnCollisionMove>();
        if (moveController != null) {
            moveController.setMove(true); // Ativa movimento do objeto.
        }
    }

}
