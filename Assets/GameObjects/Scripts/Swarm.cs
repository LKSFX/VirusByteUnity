using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm : MonoBehaviour, ISpawner {

    public float swarmSpeedFactor = 1;
    private bool completed;

    public void spawn() {
        if (completed == true) return;
        completed = true;
        // check for parent
        Transform parent = transform.parent;
        if (parent != null) {
            Swarm swarm = parent.GetComponent<Swarm>();
            if (swarm != null)
                swarm.spawn();
        }

        // spawn childs
        ISpawner spawner;
        IOnCollisionMove moveController;
        foreach (Transform go in transform) { // Itera por todos os vírus contidos neste SWARM liberando um de cada vez
            spawner = go.GetComponent<ISpawner>();
            if (spawner != null)
                spawner.spawn();
        }
    }
}
