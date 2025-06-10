using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Animators")]
    public Animator charactorAnimator;
    public Animator fishingRodAnimator;

    [Header("Rod Selection (1: Bamboo, 2: Old, 3: Iron, 4: Master)")]
    [Range(1, 4)]
    public int rodSelection = 1;

    [Header("Charactor Selection (1~2)")]
    [Range(1, 2)]
    public int charactorSelection = 1;

    private int lastRodSelection = -1;
    private int lastCharactorSelection = -1;

    private float stateTimer = 0f;
    private bool isWaiting = false;

    void Start()
    {
        ApplyRodLayer();
        ApplyCharactorLayer();
    }

    void Update()
    {
        if (rodSelection != lastRodSelection)
        {
            ApplyRodLayer();
        }

        if (charactorSelection != lastCharactorSelection)
        {
            ApplyCharactorLayer();
        }

        AnimatorStateInfo currentState = fishingRodAnimator.GetCurrentAnimatorStateInfo(rodSelection);

        if (currentState.IsName("Fishing_Idle"))
        {
            if (!isWaiting)
            {
                stateTimer = 0f;
                isWaiting = true;
            }

            stateTimer += Time.deltaTime;

            float waitTime = GetWaitTimeByRod(rodSelection);

            if (stateTimer >= waitTime)
            {
                fishingRodAnimator.SetTrigger("Fished");

                if (charactorAnimator != null)
                    charactorAnimator.SetTrigger("Fished");

                isWaiting = false;
            }
        }
        else
        {
            isWaiting = false;
            stateTimer = 0f;
        }
    }

    void ApplyRodLayer()
    {
        for (int i = 1; i <= 4; i++)
        {
            fishingRodAnimator.SetLayerWeight(i, (i == rodSelection) ? 1f : 0f);
        }

        if (fishingRodAnimator != null)
            fishingRodAnimator.SetTrigger("Reset");

        if (charactorAnimator != null)
            charactorAnimator.SetTrigger("Reset");

        lastRodSelection = rodSelection;
    }

    void ApplyCharactorLayer()
    {
        for (int i = 1; i <= 2; i++)
        {
            charactorAnimator.SetLayerWeight(i, (i == charactorSelection) ? 1f : 0f);
        }

        if (charactorAnimator != null)
            charactorAnimator.SetTrigger("Reset");

        if (fishingRodAnimator != null)
            fishingRodAnimator.SetTrigger("Reset");

        lastCharactorSelection = charactorSelection;
    }



    float GetWaitTimeByRod(int rod)
    {
        switch (rod)
        {
            case 1: return 10f;
            case 2: return 9f;
            case 3: return 7f;
            case 4: return 5f;
            default: return 10f;
        }
    }
}
