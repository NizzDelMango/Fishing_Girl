using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTN_Controller : MonoBehaviour
{
    public GameObject storePanel;
    public GameObject bucketPanel;
    public GameObject inventoryPanel;

    public GameObject MenuPanel;
    public GameObject GuidePanel;
    public GameObject SettingPanel;

    public Button storeButton;
    public Button bucketButton;
    public Button inventoryButton;

    public Button MenuButton;
    public Button GuideButton;
    public Button SettingButton;

    //  추가: BGM, SFX 슬라이더
    public Slider bgmSlider;
    public Slider sfxSlider;

    //  추가: 오디오 소스
    public AudioSource bgmAudioSource;
    public List<AudioSource> sfxAudioSources; //  SFX 여러 개 

    void Start()
    {
        storeButton.onClick.AddListener(ToggleStore);
        bucketButton.onClick.AddListener(ToggleBucket);
        inventoryButton.onClick.AddListener(ToggleInventory);

        MenuButton.onClick.AddListener(ToggleMenu);
        GuideButton.onClick.AddListener(ToggleGuide);
        SettingButton.onClick.AddListener(ToggleSetting);

        storePanel.SetActive(false);
        bucketPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        MenuPanel.SetActive(false);
        GuidePanel.SetActive(false);
        SettingPanel.SetActive(false);

        // 슬라이더 초기화 및 리스너 연결 (볼륨 저장된 거 불러오기)
        if (bgmSlider != null && bgmAudioSource != null)
        {
            float savedBGMVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
            bgmAudioSource.volume = savedBGMVolume;
            bgmSlider.value = savedBGMVolume;
            bgmSlider.onValueChanged.AddListener(ChangeBGMVolume);
        }

        if (sfxSlider != null && sfxAudioSources != null)
        {
            float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
            foreach (var sfx in sfxAudioSources)
            {
                if (sfx != null)
                    sfx.volume = savedSFXVolume;
            }
            sfxSlider.value = savedSFXVolume;
            sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GuidePanel.activeSelf)
            {
                GuidePanel.SetActive(false);
                MenuPanel.SetActive(true);
            }
            else if (SettingPanel.activeSelf)
            {
                SettingPanel.SetActive(false);
                MenuPanel.SetActive(true);
            }
            else if (storePanel.activeSelf)
            {
                storePanel.SetActive(false);
            }
            else if (bucketPanel.activeSelf)
            {
                bucketPanel.SetActive(false);
            }
            else if (inventoryPanel.activeSelf)
            {
                inventoryPanel.SetActive(false);
            }
            else if (MenuPanel.activeSelf)
            {
                MenuPanel.SetActive(false);
            }
            else
            {
                MenuPanel.SetActive(true);
                GuidePanel.SetActive(false);
                SettingPanel.SetActive(false);
            }
        }
    }

    void ToggleStore()
    {
        bool isActive = storePanel.activeSelf;
        storePanel.SetActive(!isActive);

        if (!isActive)
        {
            bucketPanel.SetActive(false);
            inventoryPanel.SetActive(false);
        }
    }

    void ToggleBucket()
    {
        bool isActive = bucketPanel.activeSelf;

        if (!isActive)
        {
            bucketPanel.SetActive(true);
            storePanel.SetActive(false);
            inventoryPanel.SetActive(false);
        }
        else
        {
            Animator animator = bucketPanel.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Bucket_Touched");
            }
        }
    }

    void ToggleInventory()
    {
        bool isActive = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isActive);

        if (!isActive)
        {
            storePanel.SetActive(false);
            bucketPanel.SetActive(false);
        }
    }

    void ToggleMenu()
    {
        MenuPanel.SetActive(!MenuPanel.activeSelf);
    }

    void ToggleGuide()
    {
        bool isActive = GuidePanel.activeSelf;
        GuidePanel.SetActive(!isActive);

        if (!isActive)
        {
            MenuPanel.SetActive(false);
            SettingPanel.SetActive(false);
        }
    }

    void ToggleSetting()
    {
        bool isActive = SettingPanel.activeSelf;
        SettingPanel.SetActive(!isActive);

        if (!isActive)
        {
            MenuPanel.SetActive(false);
            GuidePanel.SetActive(false);
        }
    }

    // 볼륨 조절 함수 (BGM)
    void ChangeBGMVolume(float volume)
    {
        if (bgmAudioSource != null)
            bgmAudioSource.volume = volume;

        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    // 볼륨 조절 함수 (SFX)
    void ChangeSFXVolume(float volume)
    {
        foreach (var sfx in sfxAudioSources)
        {
            if (sfx != null)
                sfx.volume = volume;
        }
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
