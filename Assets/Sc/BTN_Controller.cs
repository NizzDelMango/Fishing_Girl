using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTN_Controller : MonoBehaviour
{
    public GameObject storePanel;
    public GameObject inventoryPanel;

    public Button storeButton;
    public Button inventoryButton;

    void Start()
    {
        storeButton.onClick.AddListener(ToggleStore);
        inventoryButton.onClick.AddListener(ToggleInventory);

        storePanel.SetActive(false);
        inventoryPanel.SetActive(false);
    }

    void ToggleStore()
    {
        bool isActive = storePanel.activeSelf;
        storePanel.SetActive(!isActive);
        if (!isActive) inventoryPanel.SetActive(false);
    }

    void ToggleInventory()
    {
        bool isActive = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isActive);
        if (!isActive) storePanel.SetActive(false);
    }
}
