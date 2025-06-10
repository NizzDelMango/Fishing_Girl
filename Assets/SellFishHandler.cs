using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellFishHandler : MonoBehaviour
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
