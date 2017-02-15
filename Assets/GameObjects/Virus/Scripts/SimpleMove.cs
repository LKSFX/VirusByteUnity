using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : Movement {

    private void Update() {
        if (_isMoveAllowed || forceMove) {
            var dir = new Vector3(directionX, directionY, 0);
            transform.position = transform.position + dir * speed * Time.deltaTime;
        }
    }

}
