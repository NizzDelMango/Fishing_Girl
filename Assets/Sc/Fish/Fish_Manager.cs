using UnityEngine;

public class Fish_Manager : MonoBehaviour
{
    private Player_Manager playerManager;

    private string[] fishNames = {
        "멸치", "고등어", "전갱이", "쥐치", "우럭", "광어",
        "방어", "도미", "참치", "청세치", "붉바리", "백상아리"
    };

    private int[][] rodFishWeights = new int[][] {
        new int[] { 20, 15, 15, 10, 8, 8, 6, 5, 4, 3, 3, 3 }, // Bamboo
        new int[] { 15, 12, 12, 10, 9, 9, 8, 7, 5, 5, 5, 3 }, // Old
        new int[] { 10, 10, 10, 10, 10, 10, 10, 8, 6, 6, 6, 4 }, // Iron
        new int[] { 5, 5, 5, 7, 7, 7, 10, 10, 10, 10, 10, 14 }  // Master
    };

    private string[] sizeOptions = { "작은", "보통 크기의", "큰" };

    void Start()
    {
        playerManager = GameObject.FindObjectOfType<Player_Manager>();
        if (playerManager == null)
        {
            Debug.LogError("Player_Manager를 찾을 수 없습니다.");
        }
    }

    public void Fished()
    {
        if (playerManager == null) return;

        int rodIndex = playerManager.Rod;
        int[] weights = rodFishWeights[rodIndex];
        int total = 0;
        foreach (int w in weights) total += w;

        int rand = Random.Range(1, total + 1);
        int sum = 0;
        int fishIndex = 0;

        for (int i = 0; i < Mathf.Min(weights.Length, fishNames.Length); i++)
        {
            sum += weights[i];
            if (rand <= sum)
            {
                fishIndex = i;
                break;
            }
        }

        int sizeIndex = GetSizeIndexByRod(rodIndex);
        string caught = $"{sizeOptions[sizeIndex]} {fishNames[fishIndex]}";

        Debug.Log("낚은 물고기: " + caught);

        playerManager.AddFishToInventory(caught);
        playerManager.UpdateUI();
    }

    int GetSizeIndexByRod(int rodIndex)
    {
        int r = Random.Range(0, 100);
        switch (rodIndex)
        {
            case 0: return (r < 60) ? 0 : (r < 90) ? 1 : 2;
            case 1: return (r < 45) ? 0 : (r < 85) ? 1 : 2;
            case 2: return (r < 30) ? 0 : (r < 75) ? 1 : 2;
            case 3: return (r < 15) ? 0 : (r < 60) ? 1 : 2;
            default: return 1;
        }
    }

    public int GetFishPrice(int fishIndex, int sizeIndex)
    {
        int[] basePrices = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 150, 200 };
        int basePrice = basePrices[fishIndex];
        return basePrice + (sizeIndex * 10);
    }

    public string[] GetFishNames()
    {
        return fishNames;
    }
}
