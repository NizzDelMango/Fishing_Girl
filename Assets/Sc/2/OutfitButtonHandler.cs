using UnityEngine;
using UnityEngine.UI;

public class OutfitToggleButtonHandler : MonoBehaviour
{
    public GameObject shopPanel;           // 의상 선택 패널 닫기용

    private GameManager gameManager;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnToggleOutfitClicked);
        gameManager = GameManager.Instance;

        if (gameManager == null)
            Debug.LogError("GameManager가 없습니다.");
    }

    void OnToggleOutfitClicked()
    {
        if (gameManager == null) return;

        int current = gameManager.charactorSelection;
        int target = current == 1 ? 2 : 1;

        // 2번 의상이 해금되지 않은 경우 => 광고 보고 해금
        if (target == 2 && !IsOutfitUnlocked(2))
        {
            AdManager.Instance.ShowRewardedAd(() =>
            {
                UnlockOutfit(2);
                ChangeOutfit(target);
                Debug.Log("2번 의상을 광고로 해금하고 장착했습니다.");
                ClosePanel();
            });
        }
        else
        {
            // 이미 해금된 경우는 그냥 의상만 변경
            ChangeOutfit(target);
            Debug.Log($"{target}번 의상 장착 완료");
            ClosePanel();
        }
    }

    void ChangeOutfit(int index)
    {
        gameManager.charactorSelection = index;
        gameManager.SendMessage("ApplyCharactorLayer");
        gameManager.SavePlayerData();
    }

    bool IsOutfitUnlocked(int index)
    {
        return PlayerPrefs.GetInt("OutfitUnlocked_" + index, index == 1 ? 1 : 0) == 1;
    }

    void UnlockOutfit(int index)
    {
        PlayerPrefs.SetInt("OutfitUnlocked_" + index, 1);
        PlayerPrefs.Save();
    }

    void ClosePanel()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);
    }
}
