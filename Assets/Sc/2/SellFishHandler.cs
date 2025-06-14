using UnityEngine;

public class SellFishHandler : MonoBehaviour
{
    private Animator bucketAnimator;
    private GameManager gameManager;

    [Header("패널")]
    public GameObject Empty;
    public GameObject Full;

    void Start()
    {
        // 패널 상태 초기화
        Empty.SetActive(true);
        Full.SetActive(true);

        // 부모 객체에서 Animator 가져오기
        bucketAnimator = this.gameObject.GetComponent<Animator>();

        // GameManager 가져오기 "SellAllFish() 사용하기 위함"
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SellAllFish()
    {
        if (gameManager != null)
        {
            gameManager.SellAllFish();
        }
    }

    public void PlayBucketAnimation()
    {
        if (bucketAnimator != null)
        {
            bucketAnimator.SetTrigger("Bucket_Touched");
        }
    }
}
