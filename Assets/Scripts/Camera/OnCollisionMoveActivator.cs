using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionMoveActivator : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        Movement movement = collision.gameObject.GetComponent<Movement>();
        if (movement != null) {
            movement.setMove(true); // Ativa movimento do objeto.
        }
    }
}
