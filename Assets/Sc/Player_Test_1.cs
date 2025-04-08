using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Test_1 : MonoBehaviour
{
    public Animator characterAnimator;
    public GameObject unknownFishes;
    public Text expText;

    private GameObject selectedFish;
    private Animator selectedFishAnimator;
    private int level = 1, exp = 0, maxExp = 10;

    void Start()
    {
        characterAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        characterAnimator.SetInteger("Charactor_Anim", 1);
    }
}
