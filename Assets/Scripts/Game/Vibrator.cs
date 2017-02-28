using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vibrator {

#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaObject vibrator;
#endif 

    public static void vibrate() {
#if UNITY_ANDROID || UNITY_IOS
        if (isAndroid())
            vibrator.Call("vibrate");
        else if (!Application.isEditor)
            Handheld.Vibrate();
#endif
    }


    public static void vibrate(long milliseconds) {
#if UNITY_ANDROID || UNITY_IOS
        if (isAndroid())
            vibrator.Call("vibrate", milliseconds);
        else if (!Application.isEditor)
            Handheld.Vibrate();
#endif
    }

    public static void vibrate(long[] pattern, int repeat) {
#if UNITY_ANDROID || UNITY_IOS
        if (isAndroid())
            vibrator.Call("vibrate", pattern, repeat);
        else if (!Application.isEditor)
            Handheld.Vibrate();
#endif
    }

    public static bool hasVibrator() {
        return isAndroid();
    }

    public static void cancel() {
#if UNITY_ANDROID
            vibrator.Call("cancel");
#endif
    }

    private static bool isAndroid() {
#if UNITY_ANDROID && !UNITY_EDITOR
	return true;
#else
        return false;
#endif
    }
}