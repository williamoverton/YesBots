using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundCheckbox : MonoBehaviour
{
    private Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        ValueChangeCheck();
    }

    private void ValueChangeCheck()
    {
        if(StateManager.musicOn == toggle.isOn) return;

        StateManager.musicOn = toggle.isOn;

        GameObject container = GameObject.Find("MusicContainer");

        if (container)
        {
            AudioSource audio = container.GetComponent<AudioSource>();

            if (toggle.isOn)
            {
                audio.Play();
            }
            else
            {
                audio.Stop();
            }
        }else{
            Debug.Log("No music container..");
        }
    }

    void Update(){
        toggle.isOn = StateManager.musicOn;
    }
}
