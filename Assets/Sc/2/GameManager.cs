using UnityEngine;

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

    // 플레이어 정보 관련 변수
    [Header("Player Data")]
    public string playerName = "플레이어";
    public int playerLevel = 1;
    public int playerExp = 0;
    public int maxExp = 100;

    private void Awake()
    {
        // 싱글톤 생성
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadPlayerData();  // 플레이어 데이터 로드
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

        return fishNames[0]; // fallback
    }

    void SaveCaughtFish(string name, string size)
    {
        string key = $"Fish_{name}_{size}";
        int count = PlayerPrefs.GetInt(key, 0);
        PlayerPrefs.SetInt(key, count + 1);
        PlayerPrefs.Save();
    }

    // 플레이어 데이터 저장 및 불러오기
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

    private int CalculateMaxExp(int level)
    {
        return 100 + (level - 1) * 20;
    }
}
