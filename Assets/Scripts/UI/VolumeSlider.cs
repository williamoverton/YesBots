using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});

        ValueChangeCheck();
    }

    private void ValueChangeCheck(){
        StateManager.volume = slider.value;

        GameObject container = GameObject.Find("MusicContainer");

        if (container)
        {
            AudioSource audio = container.GetComponent<AudioSource>();
            
            audio.volume = StateManager.volume * 0.5f;
        }
    }

    void Update(){
        slider.value = StateManager.volume;
    }
}
