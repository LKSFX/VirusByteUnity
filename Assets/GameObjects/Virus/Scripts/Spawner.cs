using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [Tooltip("The instance to spawn at the position")]
    public GameObject instance;

    private void Start() {
        if (instance != null)
            Instantiate(instance, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
