using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Swarm : MonoBehaviour, ISpawner {

    public float swarmSpeedFactor = 1;
    public float alpha = 1;

    private bool completed;

    public void spawn() {
        if (!Application.isPlaying) return; 
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
        foreach (Transform go in transform) { // Itera por todos os vírus contidos neste SWARM liberando um de cada vez
            spawner = go.GetComponent<ISpawner>();
            if (spawner != null)
                spawner.spawn();
        }
        Destroy(gameObject); // absoleto após spawning
    }

    public void changeAlpha(Swarm swarm, float alpha) {
        if (swarm == null) return; // não aceita valor NULO
        SpriteRenderer sprite;
        foreach (Transform child in swarm.transform) {
            sprite = child.GetComponent<SpriteRenderer>();
            if (sprite != null) {
                Color c = sprite.color;
                c.a = alpha;
                sprite.color = c;
            }
            else if (child.GetComponent<Swarm>() != null) {
                changeAlpha(child.GetComponent<Swarm>(), alpha);
            }
        }
    }

    private void OnValidate() {
        alpha = Mathf.Clamp01(alpha);
        changeAlpha(this, alpha);
        //print("validate calling " + alpha);
    }

    private void OnEnable() {
        Swarm deltaSwarm = null;
        Transform nextParent = transform;
        while (nextParent != null) {
            if (nextParent.GetComponent<Swarm>() != null) {
                deltaSwarm = nextParent.GetComponent<Swarm>();
            }
            nextParent = nextParent.parent;
        }

        changeAlpha(deltaSwarm, deltaSwarm.alpha);
    }
}
