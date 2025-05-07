using UnityEngine;
using UnityEngine.UI;

public class StoreRodButton : MonoBehaviour
{
    public int rodIndex; // 0: Bamboo, 1: Old, 2: Iron, 3: Master
    public int rodPrice;
    public Player_Stats playerStats;
    public bool isAdRequired = false; // 이 낚싯대가 광고 시청으로만 얻는 것인지?

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(BuyRod);
    }

    void BuyRod()
    {
        if (playerStats == null) return;

        if (isAdRequired)
        {
            // 광고 시청 후 낚싯대 지급
            AdManager.Instance.ShowRewardedAd(() =>
            {
                playerStats.BuyFishingRod(rodIndex, 0); // 가격은 0으로 처리
                Debug.Log("광고 보상으로 낚싯대 지급!");
            });
        }
        else
        {
            // 일반 골드로 구매
            playerStats.BuyFishingRod(rodIndex, rodPrice);
        }
    }
}
