using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{

    private AudioHighPassFilter highPassFilter;
    private AudioLowPassFilter lowPassFilter;
    private AudioDistortionFilter distortionFilter;

    private float hpv;
    private float lpv;
    private float dfv;

    private void Start() {
        highPassFilter = GetComponent<AudioHighPassFilter>();
        lowPassFilter = GetComponent<AudioLowPassFilter>();
        distortionFilter = GetComponent<AudioDistortionFilter>();

        hpv = highPassFilter.cutoffFrequency;
        lpv = lowPassFilter.cutoffFrequency;
        dfv = distortionFilter.distortionLevel;
    }

    public void DisableFilters(){
        highPassFilter.cutoffFrequency = 0;
        lowPassFilter.cutoffFrequency = 22000;
        distortionFilter.distortionLevel = 0.5f;
    }

    public void EnableFilters(){
        highPassFilter.cutoffFrequency = hpv;
        lowPassFilter.cutoffFrequency = lpv;
        distortionFilter.distortionLevel = dfv;
    }
}
