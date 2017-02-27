using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusSpawner : Spawner {

    [Tooltip("The instance to spawn at the position")]
    public Virus instance;
    public float speed = 0.5f;

    private void Awake() {
        if (transform.parent != null) {
            Swarm swarm = transform.parent.GetComponent<Swarm>();
            if (swarm != null) {
                // Se o parent for um SWARM
                speed *= swarm.swarmSpeedFactor; // multiplica velocidade pela velocidade do enxame
            }
        }
    }

    public override void spawn() {
        if (instance != null) {
            Virus go = Instantiate(instance, transform.position, Quaternion.identity);
            go.setSpeed(speed);
            go.setMove(true); // força movimento ao spawnar
        }
        Destroy(gameObject);
    }

}
