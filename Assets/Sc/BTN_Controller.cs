using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BTN_Controller : MonoBehaviour
{
    [Header("패널 오브젝트")]
    public GameObject storePanel;
    public GameObject bucketPanel;
    public GameObject inventoryPanel;
    public GameObject MenuPanel;
    public GameObject GuidePanel;
    public GameObject SettingPanel;

    [Header("버튼 오브젝트")]
    public Button storeButton;
    public Button bucketButton;
    public Button inventoryButton;
    public Button MenuButton;
    public Button GuideButton;
    public Button SettingButton;
    public Button bucketFullButton;
    public Button saveExitButton;

    [Header("버킷 알림 및 플레이어 스탯")]
    public GameObject bucketFull;
    public GameManager gamemanager;
    

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

        // 초기 패널 상태 비활성화
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
            HandleEscapeInput();
        }

        
    }

    void HandleEscapeInput()
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

    /*void CheckBucketFull()
    {
        if (gamemanager == null || bucketFull == null) return;

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
    */
    void SellAllFish()
    {
        if (gamemanager != null)
        {
            gamemanager.SellAllFish();
            StartCoroutine(ActivateBucketAndPlayAnimation());
        }
    }

    IEnumerator ActivateBucketAndPlayAnimation()
    {
        bucketPanel.SetActive(true);
        yield return null; // 다음 프레임까지 대기

        Animator animator = bucketPanel.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Bucket_Touched");
        }
    }

    void SaveAndExit()
    {
        if (gamemanager != null)
        {
            gamemanager.SavePlayerData();
            Debug.Log("Player data saved successfully.");
        }
        

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
