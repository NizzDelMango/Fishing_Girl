using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Manager : MonoBehaviour
{
    [Header("Player Basic Stats")]
    public string playerName;
    public int level = 1;
    public int exp = 0;
    public int maxExp = 10;
    public int gold = 0;

    [Header("UI Elements")]
    public TMP_Text playerNameText;
    public Text levelText;
    public Text expText;
    public Slider expSlider;
    public Text goldText;

    [Header("Equipment Settings")]
    [Range(0, 1)] public int Clothes = 0;
    [Range(0, 3)] public int Rod = 0;

    [Header("References to Models")]
    public GameObject[] clothesModels;
    public GameObject[] rodModels;
    private Animator currentClothesAnimator;
    private Animator currentRodAnimator;

    [Header("Fishing and Inventory")]
    public List<string> ownedFish = new List<string>();

    private Fish_Manager fishManager;

    private int previousClothes = -1;
    private int previousRod = -1;

    void Start()
    {
        LoadPlayerData();
        ApplyModelChange();
        UpdateUI();

        fishManager = GameObject.FindObjectOfType<Fish_Manager>();
        if (fishManager == null)
            Debug.LogError("Fish_Manager를 찾을 수 없습니다.");

        StartCoroutine(FishingTriggerLoop());
    }

    void Update()
    {
        // 모델 변경 감지
        if (Clothes != previousClothes || Rod != previousRod)
        {
            ApplyModelChange();
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        ApplyModelChange();
    }
#endif

    void LoadPlayerData()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "Player");
        level = PlayerPrefs.GetInt("Level", 1);
        exp = PlayerPrefs.GetInt("Exp", 0);
        maxExp = PlayerPrefs.GetInt("MaxExp", 10);
        gold = PlayerPrefs.GetInt("Gold", 0);
    }

    public void UpdateUI()
    {
        if (playerNameText != null) playerNameText.text = playerName;
        if (levelText != null) levelText.text = "Lv. " + level;
        if (expText != null) expText.text = $"{exp} / {maxExp}";
        if (expSlider != null) expSlider.value = (float)exp / maxExp;
        if (goldText != null) goldText.text = gold.ToString();
    }

    void ApplyModelChange()
    {
        UpdateModel(clothesModels, Clothes, ref currentClothesAnimator);
        UpdateModel(rodModels, Rod, ref currentRodAnimator);

        // Reset 트리거 두 개 다 실행
        if (currentClothesAnimator != null)
            currentClothesAnimator.SetTrigger("Reset");

        if (currentRodAnimator != null)
            currentRodAnimator.SetTrigger("Reset");

        previousClothes = Clothes;
        previousRod = Rod;
    }

    void UpdateModel(GameObject[] modelArray, int index, ref Animator animatorReference)
    {
        animatorReference = null;

        for (int i = 0; i < modelArray.Length; i++)
        {
            modelArray[i].SetActive(i == index);
            if (i == index)
                animatorReference = modelArray[i].GetComponent<Animator>();
        }
    }

    public void SetClothes(int index)
    {
        if (Clothes != index)
        {
            Clothes = index;
            ApplyModelChange();
        }
    }

    public void SetRod(int index)
    {
        if (Rod != index)
        {
            Rod = index;
            ApplyModelChange();
        }
    }

    public void SavePlayerData()
    {
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("Exp", exp);
        PlayerPrefs.SetInt("MaxExp", maxExp);
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.Save();
    }

    IEnumerator FishingTriggerLoop()
    {
        float[] rodCooldowns = { 10f, 9f, 7f, 5f };

        while (true)
        {
            yield return new WaitForSeconds(rodCooldowns[Rod]);

            if (currentRodAnimator != null)
                currentRodAnimator.SetTrigger("Fished");

            if (currentClothesAnimator != null)
                currentClothesAnimator.SetTrigger("Fished");

            yield return null;

            if (fishManager != null)
                fishManager.Fished();
        }
    }

    public void AddFishToInventory(string fish)
    {
        ownedFish.Add(fish);
        int fishCount = PlayerPrefs.GetInt("FishCount", 0);
        PlayerPrefs.SetInt("FishCount", fishCount + 1);
        PlayerPrefs.Save();
        UpdateUI();
    }

    public void SellAllFish()
    {
        int totalValue = 0;
        foreach (var fish in ownedFish)
        {
            int sizeIndex = GetFishSizeIndex(fish);
            int typeIndex = GetFishTypeIndex(fish);
            totalValue += fishManager.GetFishPrice(typeIndex, sizeIndex);
        }

        gold += totalValue;
        ownedFish.Clear();
        PlayerPrefs.SetInt("FishCount", 0);
        UpdateUI();
    }

    int GetFishSizeIndex(string fishName)
    {
        if (fishName.StartsWith("작은")) return 0;
        if (fishName.StartsWith("보통")) return 1;
        if (fishName.StartsWith("큰")) return 2;
        return 1;
    }

    int GetFishTypeIndex(string fishName)
    {
        string pureName = fishName.Replace("작은 ", "").Replace("보통 크기의 ", "").Replace("큰 ", "");
        string[] fishNames = fishManager.GetFishNames();
        for (int i = 0; i < fishNames.Length; i++)
        {
            if (fishNames[i] == pureName)
                return i;
        }
        return 0;
    }
}
