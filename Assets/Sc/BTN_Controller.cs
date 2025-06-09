using UnityEngine;
using UnityEngine.UI;

public class BTN_Controller : MonoBehaviour
{
    public GameObject storePanel, bucketPanel, inventoryPanel;
    public GameObject MenuPanel, GuidePanel, SettingPanel;

    public Button storeButton, bucketButton, inventoryButton;
    public Button MenuButton, GuideButton, SettingButton;
    public GameObject bucketFull, bucketEmpty;
    public Button bucketFullButton;
    public Button saveExitButton;
    public Player_Manager playerManager;

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

        if (bucketFull != null) bucketFull.SetActive(false);
        if (bucketEmpty != null) bucketEmpty.SetActive(false);
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
            else
            {
                MenuPanel.SetActive(!MenuPanel.activeSelf);
            }
        }
    }

    void ToggleStore() => storePanel.SetActive(!storePanel.activeSelf);

    void ToggleBucket()
    {
        bool isActive = !bucketPanel.activeSelf;
        bucketPanel.SetActive(isActive);
        if (isActive) UpdateBucketVisual();
    }

    void ToggleInventory() => inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    void ToggleMenu() => MenuPanel.SetActive(!MenuPanel.activeSelf);
    void ToggleGuide() => GuidePanel.SetActive(!GuidePanel.activeSelf);
    void ToggleSetting() => SettingPanel.SetActive(!SettingPanel.activeSelf);

    void SellAllFish()
    {
        playerManager.SellAllFish();
        UpdateBucketVisual();
    }

    void SaveAndExit()
    {
        playerManager.SavePlayerData();
        Application.Quit();
    }

    void UpdateBucketVisual()
    {
        int fishCount = PlayerPrefs.GetInt("FishCount", 0);
        if (bucketFull != null) bucketFull.SetActive(fishCount > 0);
        if (bucketEmpty != null) bucketEmpty.SetActive(fishCount == 0);
    }
}
