using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEnemiesDeadObjective : MonoBehaviour
{

    private Enemy[] enemies;
    private bool triggered = false;
    public AudioClip winSound;
    private dreamloLeaderBoard dl;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindObjectsOfType<Enemy>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(triggered) return;

        for (int i = 0; i < enemies.Length; i++)
        {
            if(!enemies[i] || enemies[i].isAlive) return;
        }

        triggered = true;

        Debug.Log("Level done!");

        dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        if(dl) dl.AddScore(StateManager.playerName, Mathf.RoundToInt((Time.time - startTime) * 100));

        Debug.Log(dl.publicCode);
        Debug.Log(dl.privateCode);

        GameSceneManager gm = GameObject.FindObjectOfType<GameSceneManager>();
        if(gm){
            GameObject.FindObjectOfType<MenuSounds>().GetComponent<AudioSource>().PlayOneShot(winSound, StateManager.volume);
            gm.NextScene();
        }
    }
}
