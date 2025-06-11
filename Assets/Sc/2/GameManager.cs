using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Animators")]
    public Animator charactorAnimator;
    public Animator fishingRodAnimator;
    public Animator fishAnimator;

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

    private string[] fishNames = new string[]
    {
        "멸치", "전갱이", "쥐치", "우럭",
        "광어", "도미", "고등어", "참치",
        "방어", "붉바리", "청새치", "백상아리"
    };

    [Header("Player Data")]
    public string playerName = "플레이어";
    public int playerLevel = 1;
    public int playerExp = 0;
    public int maxExp = 100;

    [Header("UI")]
    public Slider expSlider;
    public Text expText;
    public Text levelText;
    public TMP_Text nameText;

    [Header("Player Currency")]
    public int playerGold = 0;
    public Text goldText;

    private Dictionary<string, int> fishPrices = new Dictionary<string, int>()
    {
        { "Small", 10 },
        { "Medium", 20 },
        { "Large", 40 }
    };

    public bool SpendGold(int amount)
    {
        if (playerGold >= amount)
        {
            playerGold -= amount;
            UpdateGoldUI();
            SavePlayerData();
            return true;
        }
        return false;
    }

    public void AddGold(int amount)
    {
        playerGold += amount;
        UpdateGoldUI();
        SavePlayerData();
    }

    public void UpdateGoldUI()
    {
        if (goldText != null)
            goldText.text = $"{playerGold} G";
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadPlayerData(); // [변경] Save 전에 Load
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ApplyRodLayer(); // [중요] Load 이후 적용
        ApplyCharactorLayer();
        UpdatePlayerUI();
        UpdateGoldUI();
    }

    void Update()
    {
        if (rodSelection != lastRodSelection)
            ApplyRodLayer();

        if (charactorSelection != lastCharactorSelection)
            ApplyCharactorLayer();

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
                charactorAnimator?.SetTrigger("Fished");
                fishAnimator?.SetTrigger(GetRandomSizeTrigger());

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
            fishingRodAnimator.SetLayerWeight(i, (i == rodSelection) ? 1f : 0f);

        fishingRodAnimator?.SetTrigger("Reset");
        charactorAnimator?.SetTrigger("Reset");
        lastRodSelection = rodSelection;
    }

    void ApplyCharactorLayer()
    {
        for (int i = 1; i <= 2; i++)
            charactorAnimator.SetLayerWeight(i, (i == charactorSelection) ? 1f : 0f);

        charactorAnimator?.SetTrigger("Reset");
        fishingRodAnimator?.SetTrigger("Reset");
        lastCharactorSelection = charactorSelection;
    }

    float GetWaitTimeByRod(int rod)
    {
        return rod switch
        {
            1 => 10f,
            2 => 9f,
            3 => 7f,
            4 => 5f,
            _ => 10f,
        };
    }

    string GetRandomSizeTrigger()
    {
        string[] sizeTriggers = { "Small", "Medium", "Large" };
        string size = sizeTriggers[UnityEngine.Random.Range(0, sizeTriggers.Length)];
        fishAnimator.SetTrigger(size);
        string fish = GetRandomFish();
        SaveCaughtFish(fish, size);

        int exp = size switch
        {
            "Small" => 1,
            "Medium" => 3,
            "Large" => 5,
            _ => 5
        };

        AddExp(exp);
        return size;
    }

    string GetRandomFish()
    {
        int[] weights = { 20, 18, 16, 14, 12, 10, 8, 6, 5, 4, 3, 2 };
        int totalWeight = 0;
        foreach (int w in weights)
            totalWeight += w;

        int rand = UnityEngine.Random.Range(0, totalWeight);
        int cumulative = 0;

        for (int i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i];
            if (rand < cumulative)
                return fishNames[i];
        }

        return fishNames[0];
    }

    void SaveCaughtFish(string name, string size)
    {
        string key = $"Fish_{name}_{size}";
        int count = PlayerPrefs.GetInt(key, 0);
        PlayerPrefs.SetInt(key, count + 1);

        string keyList = PlayerPrefs.GetString("FishKeys", "");
        if (!keyList.Contains(key))
        {
            keyList += key + ";";
            PlayerPrefs.SetString("FishKeys", keyList);
        }

        PlayerPrefs.Save();
    }

    public void SellAllFish()
    {
        string keyList = PlayerPrefs.GetString("FishKeys", "");
        if (string.IsNullOrEmpty(keyList)) return;

        string[] keys = keyList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        int totalGold = 0;

        foreach (string key in keys)
        {
            int count = PlayerPrefs.GetInt(key, 0);
            string[] parts = key.Split('_');
            if (parts.Length == 3)
            {
                string size = parts[2];
                if (fishPrices.ContainsKey(size))
                {
                    int price = fishPrices[size];
                    totalGold += price * count;
                }

                PlayerPrefs.DeleteKey(key);
            }
        }

        PlayerPrefs.DeleteKey("FishKeys");
        AddGold(totalGold);
        PlayerPrefs.Save();

        Debug.Log($"전체 판매 완료! 획득한 골드: {totalGold}");
    }

    public void AddExp(int amount)
    {
        playerExp += amount;

        while (playerExp >= maxExp)
        {
            playerExp -= maxExp;
            playerLevel++;
            maxExp = CalculateMaxExp(playerLevel);
        }

        UpdatePlayerUI();
        SavePlayerData();
    }

    private int CalculateMaxExp(int level)
    {
        int[] expTable = new int[]
        {
        10, 25, 50, 80, 115, 150, 200, 255, 320, 400,
        500, 610, 750, 1000, 1500, 2800, 4200, 5800, 7000, 9000,
        11000, 13500, 16500, 20000, 25000, 30000, 36000, 43000, 50000
        };

        if (level >= 1 && level <= expTable.Length)
        {
            return expTable[level - 1];
        }
        else if (level > expTable.Length && level <= 60)
        {
            int lastExp = expTable[expTable.Length - 1]; // 30레벨의 경험치: 50000
            int additionalLevel = level - expTable.Length;
            return lastExp + (additionalLevel * 10000);  // 31레벨부터는 +10000씩 증가
        }
        else
        {
            // 60레벨 이상이면 마지막 값(60레벨 기준 값) 고정
            return 50000 + (30 * 10000);  // 60레벨: 350000
        }
    }

    public void UpdatePlayerUI()
    {
        if (expSlider != null)
        {
            expSlider.maxValue = maxExp;
            expSlider.value = playerExp;
        }
        if (expText != null)
            expText.text = $"{playerExp} / {maxExp}";

        if (expText != null)
            expText.text = $"{playerExp} / {maxExp}";

        if (levelText != null)
            levelText.text = $"Lv. {playerLevel}";

        if (nameText != null)
            nameText.text = playerName;
    }

    public void LoadPlayerData()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "플레이어");
        playerLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        playerExp = PlayerPrefs.GetInt("PlayerExp", 0);
        maxExp = CalculateMaxExp(playerLevel);
        playerGold = PlayerPrefs.GetInt("PlayerGold", 0);

        rodSelection = PlayerPrefs.GetInt("RodSelection", 1); // [추가]

        UpdateGoldUI();
    }

    public void SavePlayerData()
    {
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetInt("PlayerLevel", playerLevel);
        PlayerPrefs.SetInt("PlayerExp", playerExp);
        PlayerPrefs.SetInt("PlayerGold", playerGold);
        PlayerPrefs.SetInt("RodSelection", rodSelection); // [추가]
        PlayerPrefs.Save();
    }
}
