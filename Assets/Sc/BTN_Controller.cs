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

    public GameObject bucketFull;
    public Button bucketFullButton;

    public Button saveExitButton;
    public Player_Stats playerStats;

    void Start()
    {
        storeButton.onClick.AddListener(ToggleStore);
        bucketButton.onClick.AddListener(ToggleBucket);
        inventoryButton.onClick.AddListener(ToggleInventory);

        MenuButton.onClick.AddListener(ToggleMenu);
        GuideButton.onClick.AddListener(ToggleGuide);
        SettingButton.onClick.AddListener(ToggleSetting);

        bucketFullButton.onClick.AddListener(SellAllFish);
        saveExitButton.onClick.AddListener(SaveAndExit);

        storePanel.SetActive(false);
        bucketPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        MenuPanel.SetActive(false);
        GuidePanel.SetActive(false);
        SettingPanel.SetActive(false);

        if (bucketFull != null)
            bucketFull.SetActive(false);
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

        CheckBucketFull();
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
            inventoryPanel.SetActive(true);
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
        if (GuidePanel.activeSelf || SettingPanel.activeSelf)
        {
            GuidePanel.SetActive(false);
            SettingPanel.SetActive(false);
            MenuPanel.SetActive(true);
        }
        else
        {
            MenuPanel.SetActive(!MenuPanel.activeSelf);
        }
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

    void CheckBucketFull()
    {
        if (playerStats == null || bucketFull == null) return;

        bool hasFish = false;
        foreach (Text fishText in playerStats.fishCountTexts)
        {
            if (int.Parse(fishText.text) > 0)
            {
                hasFish = true;
                break;
            }
        }

        bucketFull.SetActive(hasFish);
    }

    void SellAllFish()
    {
        if (playerStats != null)
        {
            playerStats.SellAllFish();
            StartCoroutine(ActivateBucketAndPlayAnimation());
        }
    }

    IEnumerator ActivateBucketAndPlayAnimation()
    {
        bucketPanel.SetActive(true);
        yield return null; // 다음 프레임까지 대기 (Animator가 초기화될 시간 확보)

        Animator animator = bucketPanel.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Bucket_Touched");
        }
    }
    void SaveAndExit()
{
    if (playerStats != null)
    {
        playerStats.SaveAllPlayerData();
        Debug.Log("Player data saved successfully.");
    }
    else
    {
        Debug.LogError("Player_Stats not found!");
    }

#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
}

}
