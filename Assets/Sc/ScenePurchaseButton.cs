using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScenePurchaseButton : MonoBehaviour
{
    public Player_Stats playerStats;
    public int requiredGold = 80000;
    public string sceneName = "Japan_Spring";
    private string purchaseKey;

    void Start()
    {
        purchaseKey = "Purchased_" + sceneName;

        Button btn = GetComponent<Button>();
        if (btn == null)
        {
            Debug.LogError("Button 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        btn.onClick.AddListener(TryBuyOrLoadScene);
    }

    void TryBuyOrLoadScene()
    {
        if (playerStats == null)
        {
            Debug.LogWarning("Player_Stats가 할당되지 않았습니다.");
            return;
        }

        // 이미 구매한 경우 → 골드 차감 없이 씬 이동
        if (PlayerPrefs.GetInt(purchaseKey, 0) == 1)
        {
            Debug.Log("이미 구매한 씬입니다. 바로 이동합니다.");
            SceneManager.LoadScene(sceneName);
            return;
        }

        // 아직 구매하지 않은 경우 → 골드 검사 후 구매 및 저장
        if (playerStats.gold >= requiredGold)
        {
            playerStats.gold -= requiredGold;

            PlayerPrefs.SetInt(purchaseKey, 1);
            PlayerPrefs.Save();

            Debug.Log($"골드 {requiredGold} 차감 후 {sceneName} 씬으로 이동");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }
}
