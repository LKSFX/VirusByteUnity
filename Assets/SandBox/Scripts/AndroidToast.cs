using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidToast {

    private string _deltaMessage;
    private bool _isDebug = true;
    private AndroidJavaObject _currentActivity;

	public void show(string msg) {
        showToastOnUiThread(msg);
    }

    private void showToastOnUiThread(string msg) {
#if !UNITY_EDITOR && UNITY_ANDROID
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        _currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        _deltaMessage = msg;
        _currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(toast));
#else
        Debug.Log("UnityToast -> " + msg);
#endif
    }

    private void toast() {
        if (_isDebug)
            Debug.Log("UnityToast -> Running on Ui thread");
        AndroidJavaObject context = _currentActivity.Call<AndroidJavaObject>("getApplicationContext");

        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", _deltaMessage);
        AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));

        toast.Call("show");
    }
}
