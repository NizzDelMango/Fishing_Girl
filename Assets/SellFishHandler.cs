using UnityEngine;
using UnityEngine.EventSystems;

public class SellFishHandler : MonoBehaviour, IPointerClickHandler
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // 버튼 클릭 이벤트에서 호출
    public void PlayTouchAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Bucket_Touched");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 만약에 전체 영역 클릭도 동작하게 하고 싶다면 사용
        PlayTouchAnimation();
    }

    // 애니메이션 이벤트에서 호출될 함수
    public void OnAnimationEnd()
    {
        gameObject.SetActive(false);
    }
}
