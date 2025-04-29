using UnityEngine;
using UnityEngine.UI;

public class BTN_Controller : MonoBehaviour
{
    public Button storeButton;
    public Button bucketButton;
    public Button inventoryButton;

    public GameObject storePanel;
    public GameObject bucketPanel;
    public GameObject inventoryPanel;

    public Button MenuButton;
    public Button GuideButton;
    public Button SettingButton;

    public GameObject MenuPanel;
    public GameObject GuidePanel;
    public GameObject SettingPanel;

    public Button saveExitButton;

    private Player_Stats playerStats;

    void Start()
    {
        storeButton.onClick.AddListener(ToggleStore);
        bucketButton.onClick.AddListener(ToggleBucket);
        inventoryButton.onClick.AddListener(ToggleInventory);

        MenuButton.onClick.AddListener(ToggleMenu);
        GuideButton.onClick.AddListener(ToggleGuide);
        SettingButton.onClick.AddListener(ToggleSetting);

        saveExitButton.onClick.AddListener(SaveAndExit);

        storePanel.SetActive(false);
        bucketPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        MenuPanel.SetActive(false);
        GuidePanel.SetActive(false);
        SettingPanel.SetActive(false);

        playerStats = FindObjectOfType<Player_Stats>();
    }

    void ToggleStore()
    {
        bool isActive = storePanel.activeSelf;
        storePanel.SetActive(!isActive);
    }

    void ToggleBucket()
    {
        bool isActive = bucketPanel.activeSelf;
        bucketPanel.SetActive(!isActive);

        if (bucketPanel.activeSelf)
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
    }

    void ToggleMenu()
    {
        bool isActive = MenuPanel.activeSelf;
        MenuPanel.SetActive(!isActive);
    }

    void ToggleGuide()
    {
        bool isActive = GuidePanel.activeSelf;
        GuidePanel.SetActive(!isActive);
    }

    void ToggleSetting()
    {
        bool isActive = SettingPanel.activeSelf;
        SettingPanel.SetActive(!isActive);
    }

    void SaveAndExit()
    {
        if (playerStats != null)
        {
            playerStats.SaveAllPlayerData();
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
