﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vibrator {

#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif

    public static void vibrate() {
        if (isAndroid())
            vibrator.Call("vibrate");
        else
            Handheld.Vibrate();
    }


    public static void vibrate(long milliseconds) {
        if (isAndroid())
            vibrator.Call("vibrate", milliseconds);
        else
            Handheld.Vibrate();
    }

    public static void vibrate(long[] pattern, int repeat) {
        if (isAndroid())
            vibrator.Call("vibrate", pattern, repeat);
        else
            Handheld.Vibrate();
    }

    public static bool hasVibrator() {
        return isAndroid();
    }

    public static void cancel() {
        if (isAndroid())
            vibrator.Call("cancel");
    }

    private static bool isAndroid() {
#if UNITY_ANDROID && !UNITY_EDITOR
	return true;
#else
    return false;
#endif
    }
}