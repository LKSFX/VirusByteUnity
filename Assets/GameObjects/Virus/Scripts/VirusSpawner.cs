using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusSpawner : MonoBehaviour, ISpawner {

    [Tooltip("The instance to spawn at the position")]
    public Virus instance;
    public float speed = 0.5f;

    protected virtual void Start() {
        spawn();
    }

    public void spawn() {
        if (instance != null) {
            Virus go = Instantiate(instance, transform.position, Quaternion.identity);
            go.setSpeed(speed);
        }
        Destroy(gameObject);
    }

}
