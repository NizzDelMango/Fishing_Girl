using UnityEngine;
using UnityEngine.UI;

public class StoreRodButtonHandler : MonoBehaviour
{
    public int rodIndex;                  // 낚싯대 번호 (1~4)
    public bool isAdRequired = false;     // 광고 보상 필요 여부
    public GameManager gameManager;       // 낚싯대 적용용

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

        if (isAdRequired)
        {
            // 광고 시청 후 낚싯대 지급
            AdManager.Instance.ShowRewardedAd(() =>
            {
                EquipRod();
                Debug.Log($"[광고 보상] {rodIndex}번 낚싯대 장착 완료");
            });
        }
        else
        {
            // 바로 낚싯대 장착
            EquipRod();
            Debug.Log($"{rodIndex}번 낚싯대 장착 완료");
        }
    }

    void EquipRod()
    {
        gameManager.rodSelection = rodIndex;

        // 즉시 반영을 위해 ApplyRodLayer 호출
        gameManager.SendMessage("ApplyRodLayer");
    }
}
