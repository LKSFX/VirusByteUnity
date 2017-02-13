using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Toast {
#if !UNITY_EDITOR && UNITY_ANDROID
    private static AndroidJavaClass _unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    private static AndroidJavaObject _currentActivity = _unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
#else
    private static AndroidJavaClass _unityPlayer;
    private static AndroidJavaObject _currentActivity;
#endif
    private static string _deltaMessage;
    private static bool _isDebug = true;

	public static void show(string msg) {
        showToastOnUiThread(msg);
    }

    private static void showToastOnUiThread(string msg) {
#if !UNITY_EDITOR && UNITY_ANDROID
        _deltaMessage = msg;
        _currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(toast));
#else
        Debug.Log("UnityToast -> " + msg);
#endif
    }

    private static void toast() {
        if (_isDebug)
            Debug.Log("UnityToast -> Running on Ui thread");
        AndroidJavaObject context = _currentActivity.Call<AndroidJavaObject>("getApplicationContext");

        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", _deltaMessage);
        AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));

        toast.Call("show");
    }
}
