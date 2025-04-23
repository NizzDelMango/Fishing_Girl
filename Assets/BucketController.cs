using UnityEngine;
using UnityEngine.EventSystems;

public class BucketController : MonoBehaviour, IPointerClickHandler
{
    private Animator animator;

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
    }
}
