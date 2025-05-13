using UnityEngine;
using UnityEngine.UI;

public class StoreRodButton : MonoBehaviour
{
    public int rodIndex;
    public int rodPrice;
    public Player_Stats playerStats;
    public bool isAdRequired = false;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(BuyRod);
    }

    void BuyRod()
    {
        if (playerStats == null) return;
        if (isAdRequired)
        {
            AdManager.Instance.ShowRewardedAd(() =>
            {
                playerStats.BuyFishingRod(rodIndex, 0);
                Debug.Log("광고 보상으로 낚싯대 지급!");
            });
        }
        else
        {
            playerStats.BuyFishingRod(rodIndex, rodPrice);
        }
    }
}
