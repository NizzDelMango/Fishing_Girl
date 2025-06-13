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

    public Dictionary<string, int> fishPrices = new Dictionary<string, int>()
{
    { "Small", 10 },
    { "Medium", 30 },
    { "Large", 50 }
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
        string size = GetSizeByPlayerLevel(playerLevel);
        fishAnimator.SetTrigger(size);

        string fish = GetRandomFish();
        SaveCaughtFish(fish, size);

        int exp = size switch
        {
            "Small" => 1,
            "Medium" => 3,
            "Large" => 5,
            _ => 1
        };

        AddExp(exp);
        return size;
    }

    string GetSizeByPlayerLevel(int level)
    {
        float rand = UnityEngine.Random.Range(0f, 100f);

        if (level == 1)
        {
            if (rand < 99f) return "Small";
            else return "Medium";
        }
        else if (level == 2)
        {
            if (rand < 98f) return "Small";
            else if (rand < 100f) return "Medium";
            else return "Large";
        }
        else if (level == 3)
        {
            if (rand < 97f) return "Small";
            else if (rand < 100f) return "Medium";
            else return "Large";
        }
        else if (level == 4 || level == 5)
        {
            if (rand < 97.9f) return "Small";
            else if (rand < 99.9f) return "Medium";
            else return "Large";
        }
        else if (level == 6)
        {
            if (rand < 96.9f) return "Small";
            else if (rand < 99.9f) return "Medium";
            else return "Large";
        }
        else if (level == 7)
        {
            if (rand < 95.9f) return "Small";
            else if (rand < 99.9f) return "Medium";
            else return "Large";
        }
        else if (level == 8)
        {
            if (rand < 94.9f) return "Small";
            else if (rand < 99.9f) return "Medium";
            else return "Large";
        }
        else if (level == 9)
        {
            if (rand < 93.9f) return "Small";
            else if (rand < 98.9f) return "Medium";
            else return "Large";
        }
        else if (level == 10)
        {
            if (rand < 92.9f) return "Small";
            else if (rand < 98.9f) return "Medium";
            else return "Large";
        }
        else if (level <= 13)
        {
            if (rand < 90f) return "Small";
            else if (rand < 98f) return "Medium";
            else return "Large";
        }
        else if (level == 14)
        {
            if (rand < 80f) return "Small";
            else if (rand < 95f) return "Medium";
            else return "Large";
        }
        else if (level <= 17)
        {
            if (rand < 70f) return "Small";
            else if (rand < 95f) return "Medium";
            else return "Large";
        }
        else if (level <= 19)
        {
            if (rand < 65f) return "Small";
            else if (rand < 90f) return "Medium";
            else return "Large";
        }
        else if (level == 20)
        {
            if (rand < 64f) return "Small";
            else if (rand < 89f) return "Medium";
            else return "Large";
        }
        else if (level == 21)
        {
            if (rand < 63f) return "Small";
            else if (rand < 88f) return "Medium";
            else return "Large";
        }
        else if (level == 22)
        {
            if (rand < 60f) return "Small";
            else if (rand < 88f) return "Medium";
            else return "Large";
        }
        else if (level == 23)
        {
            if (rand < 58f) return "Small";
            else if (rand < 88f) return "Medium";
            else return "Large";
        }
        else if (level == 24)
        {
            if (rand < 55f) return "Small";
            else if (rand < 87f) return "Medium";
            else return "Large";
        }
        else if (level == 25)
        {
            if (rand < 50f) return "Small";
            else if (rand < 85f) return "Medium";
            else return "Large";
        }
        else if (level == 26)
        {
            if (rand < 45f) return "Small";
            else if (rand < 83f) return "Medium";
            else return "Large";
        }
        else if (level == 27)
        {
            if (rand < 60f) return "Small";
            else if (rand < 80f) return "Medium";
            else return "Large";
        }
        else if (level == 28)
        {
            if (rand < 35f) return "Small";
            else if (rand < 77f) return "Medium";
            else return "Large";
        }
        else if (level == 29)
        {
            if (rand < 30f) return "Small";
            else if (rand < 75f) return "Medium";
            else return "Large";
        }
        else if (level == 30)
        {
            if (rand < 25f) return "Small";
            else if (rand < 75f) return "Medium";
            else return "Large";
        }

        return "Small";
    }




    string GetRandomFish()
    {
        int index = UnityEngine.Random.Range(0, fishNames.Length);
        return fishNames[index];
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
        charactorSelection = PlayerPrefs.GetInt("CharactorSelection", 1);
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
        PlayerPrefs.SetInt("CharactorSelection", charactorSelection);
        PlayerPrefs.Save();
    }
}
