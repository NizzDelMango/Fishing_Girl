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
        // Guide나 Setting이 열려있으면 끄고 MenuPanel만 열어
        if (GuidePanel.activeSelf || SettingPanel.activeSelf)
        {
            GuidePanel.SetActive(false);
            SettingPanel.SetActive(false);
            MenuPanel.SetActive(true);
        }
        else
        {
            // 그냥 MenuPanel을 토글
            MenuPanel.SetActive(!MenuPanel.activeSelf);
        }
    }

    void ToggleGuide()
    {
        bool isActive = GuidePanel.activeSelf;
        GuidePanel.SetActive(!isActive);

        if (!isActive) // Guide를 켜는 경우
        {
            MenuPanel.SetActive(false);
            SettingPanel.SetActive(false);
        }
    }

    void ToggleSetting()
    {
        bool isActive = SettingPanel.activeSelf;
        SettingPanel.SetActive(!isActive);

        if (!isActive) // Setting을 켜는 경우
        {
            MenuPanel.SetActive(false);
            GuidePanel.SetActive(false);
        }
    }
}
