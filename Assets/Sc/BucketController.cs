using UnityEngine;
using UnityEngine.EventSystems;

public class BucketController : MonoBehaviour, IPointerClickHandler
{
    private Animator animator;
    public GameObject bucketEmpty;
    public GameObject bucketFull;

    void Start()
    {
        animator = GetComponent<Animator>();
        UpdateBucketState();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (animator != null)
            animator.SetTrigger("Bucket_Touched");
    }

    public void OnAnimationEnd()
    {
        gameObject.SetActive(false);
    }

    public void UpdateBucketState()
    {
        int fishCount = PlayerPrefs.GetInt("FishCount", 0);
        bucketEmpty.SetActive(fishCount == 0);
        bucketFull.SetActive(fishCount > 0);
    }
}
