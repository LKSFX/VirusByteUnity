using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Spawner : MonoBehaviour, ISpawner, IOnCollisionMove {

    private void Awake() {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }

    /// <summary>
    /// Quando qualquer um dos spawners presentes entrar em contato com o detector da câmera
    /// todos os outros vírus presentes neste swarm serão spawnados
    /// </summary>
    /// <param name="move"></param>
    public void setMove(bool move) {
        Transform parent = transform.parent;
        Swarm swarm = parent.GetComponent<Swarm>();
        if (swarm != null)
            swarm.spawn();
    }

    public virtual void spawn() {
        Destroy(gameObject); // Spawner swarm não é mais necessário após liberar os vírus
    }

}
