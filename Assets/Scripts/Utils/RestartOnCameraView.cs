using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartOnCameraView : MonoBehaviour {

    private void OnBecameVisible() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
