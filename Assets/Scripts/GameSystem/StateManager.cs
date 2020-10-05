using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class StateManager
{
    public static float volume = 0.2f;
    public static bool musicOn = false;
    public static bool paused = false;
    public static string playerName = "Player";

    public static void Pause(bool shouldPause){
        if(shouldPause){
            Time.timeScale = 0f;
        }else{
            Time.timeScale = 1f;
        }

        paused = shouldPause;
    }
}