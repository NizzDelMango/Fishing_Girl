using UnityEngine;
using UnityEngine.UI;

public class OutfitToggleButtonHandler : MonoBehaviour
{
    public int outfit2Price = 100;         // 2번 의상 해금 비용
    public GameObject shopPanel;           // 패널 닫기용 (옵션)

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

        if (target == 2 && !IsOutfitUnlocked(2))
        {
            if (gameManager.SpendGold(outfit2Price))
            {
                UnlockOutfit(2);
                ChangeOutfit(target);
                Debug.Log("2번 의상을 해금하고 장착했습니다.");
            }
            else
            {
                Debug.Log("골드가 부족하여 2번 의상을 해금할 수 없습니다.");
            }
        }
        else
        {
            ChangeOutfit(target);
            Debug.Log($"{target}번 의상 장착 완료");
        }

        ClosePanel();
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
