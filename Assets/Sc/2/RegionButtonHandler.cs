using UnityEngine;
using UnityEngine.UI;

public class RegionButtonHandler : MonoBehaviour
{
    public string regionName;                 // 지역 이름 (예: "Sea", "Forest")
    public int unlockCost = 0;              // 지역 해금 비용
    public Sprite regionBackground;           // 바꿀 배경 이미지
    public Image backgroundImage;             // 변경할 대상 이미지 (직접 드래그로 할당)

    private Button regionButton;
    private GameManager gameManager;

    void Start()
    {
        regionButton = GetComponent<Button>();
        regionButton.onClick.AddListener(OnRegionButtonClicked);

        gameManager = GameManager.Instance;

        if (backgroundImage == null)
        {
            Debug.LogError("backgroundImage가 할당되지 않았습니다. 인스펙터에서 연결해주세요.");
        }

        if (gameManager == null)
        {
            Debug.LogError("GameManager가 씬에 존재하지 않습니다.");
        }
    }

    void OnRegionButtonClicked()
    {
        if (backgroundImage == null || regionBackground == null) return;

        if (IsRegionUnlocked())
        {
            ChangeBackground();
        }
        else
        {
            if (gameManager.SpendGold(unlockCost))
            {
                UnlockRegion();
                ChangeBackground();
                Debug.Log($"{regionName} 지역이 해금되고 배경이 변경되었습니다.");
            }
            else
            {
                Debug.Log("골드가 부족하여 해당 지역을 해금할 수 없습니다.");
            }
        }
    }

    bool IsRegionUnlocked()
    {
        return PlayerPrefs.GetInt("RegionUnlocked_" + regionName, 0) == 1;
    }

    void UnlockRegion()
    {
        PlayerPrefs.SetInt("RegionUnlocked_" + regionName, 1);
        PlayerPrefs.Save();
    }

    void ChangeBackground()
    {
        backgroundImage.sprite = regionBackground;
    }
}
