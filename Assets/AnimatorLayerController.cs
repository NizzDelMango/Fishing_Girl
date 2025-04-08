using UnityEngine;
using System.Collections;

public class AnimatorLayerController : MonoBehaviour
{
    public Animator animator;
    public int layer = 0; // 변경할 레이어 (기본값 0)
    [Range(0f, 1f)] public float goalWeight = 1f; // 목표 가중치
    public float blendDuration = 0f; // 가중치 변경 시간 (0이면 즉시 변경)

    private void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
    }

    public void SetLayerWeight()
    {
        if (blendDuration > 0)
            StartCoroutine(BlendLayerWeight(layer, goalWeight, blendDuration));
        else
            animator.SetLayerWeight(layer, goalWeight);
    }

    private IEnumerator BlendLayerWeight(int layerIndex, float targetWeight, float duration)
    {
        float startWeight = animator.GetLayerWeight(layerIndex);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newWeight = Mathf.Lerp(startWeight, targetWeight, elapsedTime / duration);
            animator.SetLayerWeight(layerIndex, newWeight);
            yield return null;
        }

        animator.SetLayerWeight(layerIndex, targetWeight);
    }
}
