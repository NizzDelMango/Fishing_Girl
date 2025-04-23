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

    public Button storeButton;
    public Button bucketButton;
    public Button inventoryButton;
    public Button MenuButton;

    void Start()
    {
        storeButton.onClick.AddListener(ToggleStore);
        bucketButton.onClick.AddListener(ToggleBucket);
        inventoryButton.onClick.AddListener(ToggleInventory);
        MenuButton.onClick.AddListener(ToggleMenu);

        storePanel.SetActive(false);
        bucketPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        MenuPanel.SetActive(false);
    }

    void ToggleStore()
    {
        bool isActive = storePanel.activeSelf;
        storePanel.SetActive(!isActive);
        if (!isActive)
        {
            inventoryPanel.SetActive(false);
            bucketPanel.SetActive(false);
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
            // 이미 켜져 있으면 애니메이션 Trigger
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
            bucketPanel.SetActive (false);
        }
    }

    void ToggleMenu()
    {
        bool isActive = MenuPanel.activeSelf;
        MenuPanel.SetActive(!isActive);
    }
}
