using UnityEngine;
using UnityEngine.UI;

public class StoreRodButtonHandler : MonoBehaviour
{
    public int rodIndex;                  // ³¬½Ë´ë ¹øÈ£ (1~4)
    public bool isAdRequired = false;     // ±¤°í º¸»ó ÇÊ¿ä ¿©ºÎ
    public GameManager gameManager;       // ³¬½Ë´ë Àû¿ë¿ë

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnRodButtonClicked);
    }

    void OnRodButtonClicked()
    {
        if (gameManager == null)
        {
            Debug.LogWarning("GameManager°¡ ¿¬°áµÇÁö ¾Ê¾Ò½À´Ï´Ù.");
            return;
        }

        if (isAdRequired)
        {
            // ±¤°í ½ÃÃ» ÈÄ ³¬½Ë´ë Áö±Þ
            AdManager.Instance.ShowRewardedAd(() =>
            {
                gameManager.rodSelection = rodIndex;
                Debug.Log($"[±¤°í º¸»ó] {rodIndex}¹ø ³¬½Ë´ë ÀåÂø ¿Ï·á");
            });
        }
        else
        {
            // ¹Ù·Î ³¬½Ë´ë ÀåÂø
            gameManager.rodSelection = rodIndex;
            Debug.Log($"{rodIndex}¹ø ³¬½Ë´ë ÀåÂø ¿Ï·á");
        }
    }
}
