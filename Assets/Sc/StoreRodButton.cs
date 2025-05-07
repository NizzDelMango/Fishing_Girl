using UnityEngine;
using UnityEngine.UI;

public class StoreRodButton : MonoBehaviour
{
    public int rodIndex; // 0: Bamboo, 1: Old, 2: Iron, 3: Master
    public int rodPrice;
    public Player_Stats playerStats;
    public bool isAdRequired = false; // �� ���˴밡 ���� ��û���θ� ��� ������?

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(BuyRod);
    }

    void BuyRod()
    {
        if (playerStats == null) return;

        if (isAdRequired)
        {
            // ���� ��û �� ���˴� ����
            AdManager.Instance.ShowRewardedAd(() =>
            {
                playerStats.BuyFishingRod(rodIndex, 0); // ������ 0���� ó��
                Debug.Log("���� �������� ���˴� ����!");
            });
        }
        else
        {
            // �Ϲ� ���� ����
            playerStats.BuyFishingRod(rodIndex, rodPrice);
        }
    }
}
