using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadPlayerData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ApplyRodLayer();
        ApplyCharactorLayer();
        UpdatePlayerUI();
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

                if (charactorAnimator != null)
                    charactorAnimator.SetTrigger("Fished");

                if (fishAnimator != null)
                    TriggerRandomFishAnimation();

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

        fishingRodAnimator?.SetTrigger("Reset");
        charactorAnimator?.SetTrigger("Reset");

        lastRodSelection = rodSelection;
    }

    void ApplyCharactorLayer()
    {
        for (int i = 1; i <= 2; i++)
        {
            charactorAnimator.SetLayerWeight(i, (i == charactorSelection) ? 1f : 0f);
        }

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

    void TriggerRandomFishAnimation()
    {
        string[] sizeTriggers = { "Small", "Medium", "Large" };
        string size = sizeTriggers[Random.Range(0, sizeTriggers.Length)];

        fishAnimator.SetTrigger(size);
        Debug.Log($"Fish Animation Triggered: {size}");

        string fish = GetRandomFish();
        SaveCaughtFish(fish, size);

        Debug.Log($"잡힌 물고기: {fish} ({size})");

        int exp = size switch
        {
            "Small" => 1,
            "Medium" => 3,
            "Large" => 5,
            _ => 5
        };

        AddExp(exp);
    }

    string GetRandomFish()
    {
        int[] weights = { 20, 18, 16, 14, 12, 10, 8, 6, 5, 4, 3, 2 };

        int totalWeight = 0;
        foreach (int w in weights)
            totalWeight += w;

        int rand = Random.Range(0, totalWeight);
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
        PlayerPrefs.Save();
    }

    public void AddExp(int amount)
    {
        playerExp += amount;

        while (playerExp >= maxExp)
        {
            playerExp -= maxExp;
            playerLevel++;

            maxExp = CalculateMaxExp(playerLevel);
            Debug.Log($"레벨업! 현재 레벨: {playerLevel}");
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
            return expTable[level - 1];

        return expTable[expTable.Length - 1]; // 레벨 30 이상은 고정 또는 자유 처리
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

        Debug.Log($"플레이어 데이터 불러옴: {playerName}, 레벨 {playerLevel}, 경험치 {playerExp}/{maxExp}");
    }

    public void SavePlayerData()
    {
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetInt("PlayerLevel", playerLevel);
        PlayerPrefs.SetInt("PlayerExp", playerExp);
        PlayerPrefs.Save();

        Debug.Log("플레이어 데이터 저장됨.");
    }
}
