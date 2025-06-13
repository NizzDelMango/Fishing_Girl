using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegionButtonHandler : MonoBehaviour
{
    public string sceneName;          // 이동할 씬 이름
    public int unlockCost = 100;      // 해당 지역 해금 비용

    private Button regionButton;
    private GameManager gameManager;

    void Start()
    {
        regionButton = GetComponent<Button>();
        regionButton.onClick.AddListener(OnRegionButtonClicked);

        gameManager = GameManager.Instance;

        if (gameManager == null)
        {
            Debug.LogError("GameManager가 씬에 존재하지 않습니다.");
        }
    }

    void OnRegionButtonClicked()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (sceneName == currentScene)
        {
            Debug.Log("이미 현재 지역에 있습니다.");
            return;
        }

        if (IsRegionUnlocked())
        {
            LoadRegion();
        }
        else
        {
            if (gameManager.SpendGold(unlockCost))
            {
                UnlockRegion();
                LoadRegion();
                Debug.Log($"{sceneName} 지역이 해금되고 이동했습니다.");
            }
            else
            {
                Debug.Log("골드가 부족하여 해당 지역을 해금할 수 없습니다.");
            }
        }
    }

    bool IsRegionUnlocked()
    {
        return PlayerPrefs.GetInt("RegionUnlocked_" + sceneName, 0) == 1;
    }

    void UnlockRegion()
    {
        PlayerPrefs.SetInt("RegionUnlocked_" + sceneName, 1);
        PlayerPrefs.Save();
    }

    void LoadRegion()
    {
        SceneManager.LoadScene(sceneName);
    }
}
