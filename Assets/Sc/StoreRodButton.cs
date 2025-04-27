using UnityEngine;
using UnityEngine.UI;

public class StoreRodButton : MonoBehaviour
{
    public int rodIndex; // 0: Bamboo, 1: Old, 2: Iron, 3: Master
    public int rodPrice;
    public Player_Stats playerStats;

    void Start()
    {
        // 버튼에 클릭 리스너 추가
        GetComponent<Button>().onClick.AddListener(BuyRod);
    }

    void BuyRod()
    {
        if (playerStats != null)
        {
            playerStats.BuyFishingRod(rodIndex, rodPrice);
        }
    }
}
