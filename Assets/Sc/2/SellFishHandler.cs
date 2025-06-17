using UnityEngine;
using UnityEngine.UI; // 버튼 제어용

public class SellFishHandler : MonoBehaviour
{
    private Animator bucketAnimator;
    private GameManager gameManager;

    [Header("패널")]
    public GameObject Empty;
    public GameObject Full;

    [Header("버튼 그룹")] // 자식 버튼들을 여기에 넣기
    public GameObject[] childButtons; // 또는 Button[] 으로 해도 됨

    private bool buttonsVisible = false;

    void Start()
    {
        // 패널 상태 초기화
        Empty.SetActive(true);
        Full.SetActive(true);

        // 부모 객체에서 Animator 가져오기
        bucketAnimator = this.gameObject.GetComponent<Animator>();

        // GameManager 가져오기 "SellAllFish() 사용하기 위함"
        gameManager = FindObjectOfType<GameManager>();

        // 처음에는 자식 버튼들 비활성화
        SetChildButtonsActive(false);
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

        // 버튼 토글
        buttonsVisible = !buttonsVisible;
        SetChildButtonsActive(buttonsVisible);
    }

    private void SetChildButtonsActive(bool isActive)
    {
        foreach (GameObject buttonObj in childButtons)
        {
            buttonObj.SetActive(isActive);
        }
    }
}
