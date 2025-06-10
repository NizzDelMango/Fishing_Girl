using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider bgmSlider;
    public Slider sfxSlider;

    void Start()
    {
        float savedBGM = PlayerPrefs.GetFloat("BGM", 1.0f);
        float savedSFX = PlayerPrefs.GetFloat("SFX", 1.0f);

        mixer.SetFloat("BGM", Mathf.Log10(savedBGM) * 20);
        mixer.SetFloat("SFX", Mathf.Log10(savedSFX) * 20);

        if (bgmSlider != null) bgmSlider.value = savedBGM;
        if (sfxSlider != null) sfxSlider.value = savedSFX;  
    }

    public void BGMLevel(float sliderValue)
    {
        float savedSFXVolume = PlayerPrefs.GetFloat("BGM",1.0f);
        mixer.SetFloat("BGM", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.Save();
    }
    public void SFXLevel(float sliderValue)
    {
        float savedSFXVolume = PlayerPrefs.GetFloat("SFX",1.0f);
        mixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.Save();
    }
 
    

}
