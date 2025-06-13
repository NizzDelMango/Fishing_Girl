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
        Full.SetActive(false);

        // 부모 객체에서 Animator 가져오기
        bucketAnimator = this.gameObject.GetComponent<Animator>();

        // GameManager 가져오기 "SellAllFishi() 사용하기 위함"
        gameManager = FindObjectOfType<GameManager>();
    }

    //public void PlayBucketAnimation() // 애니메이션 동작 로직
    //{
    //    if (!this.gameObject.activeSelf)
    //    {
    //        // 꺼져 있으면 켜기
    //        this.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        // 켜져 있으면 애니메이션 실행 후 꺼짐
    //        bucketAnimator.SetTrigger("Bucket_Touched");
    //        StartCoroutine(DisableAfterAnimation());
    //    }
    //}

    public void SellAllFish()
    {
        if (gameManager != null)
        {
            gameManager.SellAllFish();  // GameManager.cs 안에 있는 SellAllFish 호출
        }

        //// 애니메이션 실행 후 꺼짐
        //bucketAnimator.SetTrigger("Bucket_Touched");
        //StartCoroutine(DisableAfterAnimation());
    }

    //private System.Collections.IEnumerator DisableAfterAnimation()
    //{
    //    // 애니메이션 길이만큼 대기 후 꺼짐
    //    float animTime = bucketAnimator.GetCurrentAnimatorStateInfo(0).length;
    //    yield return new WaitForSeconds(animTime);
    //    this.gameObject.SetActive(false);
    //}
}
