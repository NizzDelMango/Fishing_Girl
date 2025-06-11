using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [Header("Animators to Reset")]
    public Animator characterAnimator;
    public Animator fishAnimator;
    public Animator rodAnimator;

    void Start()
    {
        if (characterAnimator != null)
            characterAnimator.SetTrigger("Reset");

        if (rodAnimator != null)
            rodAnimator.SetTrigger("Reset");
    }
}
