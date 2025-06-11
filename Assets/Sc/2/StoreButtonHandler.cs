using UnityEngine;
using UnityEngine.UI;

public class StoreRodButtonHandler : MonoBehaviour
{
    public int rodIndex;                    // 낚싯대 번호 (1~4)
    public int rodPrice = 100;              // 낚싯대 가격 (골드)
    public bool isAdRequired = false;       // 광고 보상 필요 여부
    public GameManager gameManager;         // GameManager 참조

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnRodButtonClicked);
    }

    void OnRodButtonClicked()
    {
        if (gameManager == null)
        {
            Debug.LogWarning("GameManager가 연결되지 않았습니다.");
            return;
        }

        // 이미 구매했는지 확인
        if (IsRodPurchased())
        {
            Debug.Log($"이미 구매한 {rodIndex}번 낚싯대입니다.");
            EquipRod();  // 장착만 허용
            return;
        }

        if (isAdRequired)
        {
            // 광고 보상 낚싯대
            AdManager.Instance.ShowRewardedAd(() =>
            {
                SaveRodPurchase();   // 광고 본 후 구매 처리
                EquipRod();
                Debug.Log($"[광고 보상] {rodIndex}번 낚싯대 장착 완료");
            });
        }
        else
        {
            // 골드로 구매하는 낚싯대
            if (gameManager.playerGold >= rodPrice)
            {
                gameManager.playerGold -= rodPrice;
                SaveRodPurchase();   // 구매 처리
                EquipRod();
                Debug.Log($"{rodIndex}번 낚싯대 구매 및 장착 완료");
            }
            else
            {
                Debug.Log("골드가 부족하여 낚싯대를 구매할 수 없습니다.");
            }
        }
    }

    void EquipRod()
    {
        gameManager.rodSelection = rodIndex;
        gameManager.SendMessage("ApplyRodLayer");  // 선택 반영
    }

    bool IsRodPurchased()
    {
        return PlayerPrefs.GetInt("RodPurchased_" + rodIndex, 0) == 1;
    }

    void SaveRodPurchase()
    {
        PlayerPrefs.SetInt("RodPurchased_" + rodIndex, 1);
        PlayerPrefs.Save();
    }
}
