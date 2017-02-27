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
        Transform swarm = transform.parent;
        ISpawner spawner;
        IOnCollisionMove moveController;
        foreach (Transform go in swarm) { // Itera por todos os vírus contidos neste SWARM liberando um de cada vez
            spawner = go.GetComponent<ISpawner>();
            moveController = go.GetComponent<IOnCollisionMove>();
            if (spawner != null)
                spawner.spawn();
        }
        Destroy(swarm.gameObject); // Spawner swarm não é mais necessário após liberar os vírus
    }

    public virtual void spawn() {
        
    }

}
