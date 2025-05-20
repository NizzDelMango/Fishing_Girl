using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;

    public void BGMLevel(float sliderValue)
    {
        float savedSFXVolume = PlayerPrefs.GetFloat("BGM",1.0f);
        mixer.SetFloat("BGM", Mathf.Log10(sliderValue) * 20);
    }
    public void SFXLevel(float sliderValue)
    {
        float savedSFXVolume = PlayerPrefs.GetFloat("SFX",1.0f);
        mixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);
    }
    

}
