using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public GameObject screen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape")){
            StateManager.Pause(!StateManager.paused);
        }

        if(screen.activeSelf != StateManager.paused) screen.SetActive(StateManager.paused);
    }
}
