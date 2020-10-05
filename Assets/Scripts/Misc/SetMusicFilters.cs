using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMusicFilters : MonoBehaviour
{
    public bool filtersOn;

    // Start is called before the first frame update
    void Start()
    {
        MusicController music = GameObject.FindObjectOfType<MusicController>();

        if(music){
            if(filtersOn) {
                music.EnableFilters();
            }else{
                music.DisableFilters();
            }
        }
    }
}
