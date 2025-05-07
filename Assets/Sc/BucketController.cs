using UnityEngine;
using UnityEngine.EventSystems;

public class BucketController : MonoBehaviour, IPointerClickHandler
{
    private Animator animator;

    public GameObject inventoryPanel;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (animator != null)
        {
            animator.SetTrigger("Bucket_Touched");
        }
    }

    public void OnAnimationEnd()
    {
        gameObject.SetActive(false);
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false); // 인벤토리 패널도 같이 비활성화
        }
    }
}
