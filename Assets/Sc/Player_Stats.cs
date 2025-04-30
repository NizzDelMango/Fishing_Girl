using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_Stats : MonoBehaviour
{
    [Header("UI")]
    public Animator characterAnimator;
    public Text expText, levelText;
    public Slider expSlider;
    public Text[] fishCountTexts = new Text[36];
    public Text goldText;

    [Header("UI Indicator")]
    public GameObject bucketFullIndicator;

    [Header("Player Name")]
    public TMP_InputField nameInputField;
    public TextMeshProUGUI playerNameDisplay;

    [Header("Stats")]
    [Range(0, 3)]
    public int equippedRodIndex = 0;
    public int gold = 100000;

    [Header("Sound")]
    public GameObject coinSoundObject;
    private AudioSource coinAudioSource;

    private int level = 1, exp = 0, maxExp = 10;
    private bool isFishing = false;
    private Coroutine fishingRoutine;
    private int previousRodIndex = -1;

    private readonly string[] rodNames = {
        "Bamboo_fishing_rod", "Old_fishing_rod", "Iron_fishing_rod", "Master_fishing_rod"
    };

    private float[] rodTimes = { 10f, 9f, 7f, 5f };

    void Start()
    {
        level = SaveManager.LoadInt("Level", 1);
        exp = SaveManager.LoadInt("Exp", 0);
        gold = SaveManager.LoadInt("Gold", 0);
        equippedRodIndex = SaveManager.LoadInt("RodIndex", 0);

        int[] loadedFishCounts = SaveManager.LoadFishCounts(fishCountTexts.Length);
        for (int i = 0; i < fishCountTexts.Length; i++)
            fishCountTexts[i].text = loadedFishCounts[i].ToString();

        UpdateRod();
        UpdateExpUI();
        UpdateGoldUI();
        CheckBucketStatus();

        if (coinSoundObject != null)
            coinAudioSource = coinSoundObject.GetComponent<AudioSource>();

        if (!isFishing)
            fishingRoutine = StartCoroutine(AutoFishingLoop());

        if (PlayerPrefs.HasKey("PlayerName"))
        {
            string playerName = PlayerPrefs.GetString("PlayerName");
            playerNameDisplay.text = playerName;
            nameInputField.text = playerName;
        }
    }

    void Update()
    {
        UpdateRod();

        if (characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fishing"))
            characterAnimator.SetInteger("Fish", 0);
    }

    void OnApplicationQuit()
    {
        SaveAllPlayerData();
    }

    public void OnNameChanged()
    {
        if (nameInputField != null && nameInputField.text.Length > 1)
        {
            string newName = nameInputField.text;
            PlayerPrefs.SetString("PlayerName", newName);
            PlayerPrefs.Save();
            playerNameDisplay.text = newName;
            Debug.Log("플레이어 이름 변경됨: " + newName);
        }
        else
        {
            Debug.LogWarning("이름은 2자 이상이어야 합니다!");
        }
    }

    IEnumerator AutoFishingLoop()
    {
        while (true)
        {
            if (!isFishing)
                fishingRoutine = StartCoroutine(FishingProcess());

            yield return new WaitUntil(() => !isFishing);
            yield return new WaitForSeconds(0.5f);
        }
    }

    void UpdateGoldUI()
    {
        goldText.text = gold.ToString();
    }

    void UpdateRod()
    {
        equippedRodIndex = Mathf.Clamp(equippedRodIndex, 0, rodNames.Length - 1);
        characterAnimator.SetInteger("Fishing_rod", equippedRodIndex);

        if (previousRodIndex != equippedRodIndex)
        {
            if (isFishing && fishingRoutine != null)
            {
                StopCoroutine(fishingRoutine);
                isFishing = false;
            }

            characterAnimator.SetInteger("Fish", 0);

            if (fishingRoutine != null)
                StopCoroutine(fishingRoutine);
            fishingRoutine = StartCoroutine(AutoFishingLoop());

            previousRodIndex = equippedRodIndex;
        }

        for (int i = 1; i <= 4; i++)
            characterAnimator.SetLayerWeight(i, (i - 1 == equippedRodIndex) ? 1f : 0f);
    }

    float GetRodAnimationTime()
    {
        return rodTimes[equippedRodIndex];
    }

    IEnumerator FishingProcess()
    {
        isFishing = true;
        characterAnimator.SetInteger("Fish", 0);

        float time = GetRodAnimationTime();
        float elapsed = 0f;

        while (elapsed < time)
        {
            if (!isFishing) yield break;
            elapsed += Time.deltaTime;
            yield return null;
        }

        int fishType = GetFishByLevel();
        characterAnimator.SetInteger("Fish", fishType);
        GainExp(fishType);

        isFishing = false;
    }

    int GetFishByLevel()
    {
        float rand = Random.Range(0f, 100f);

        if (level == 1) return rand < 99f ? 3 : rand < 100f ? 2 : 1;
        if (level == 2) return rand < 98f ? 3 : rand < 100f ? 2 : 1;
        if (level == 3) return rand < 97f ? 3 : rand < 100f ? 2 : 1;
        if (level == 4 || level == 5) return rand < 97.9f ? 3 : rand < 99.9f ? 2 : 1;
        if (level == 6) return rand < 96.9f ? 3 : rand < 99.9f ? 2 : 1;
        if (level == 7) return rand < 95.9f ? 3 : rand < 99.9f ? 2 : 1;
        if (level == 8) return rand < 94.9f ? 3 : rand < 99.9f ? 2 : 1;
        if (level == 9) return rand < 93.9f ? 3 : rand < 98.9f ? 2 : 1;
        if (level == 10) return rand < 92.9f ? 3 : rand < 98.9f ? 2 : 1;
        if (level <= 13) return rand < 90f ? 3 : rand < 98f ? 2 : 1;
        if (level == 14) return rand < 80f ? 3 : rand < 95f ? 2 : 1;
        if (level <= 17) return rand < 70f ? 3 : rand < 95f ? 2 : 1;
        if (level <= 19) return rand < 65f ? 3 : rand < 90f ? 2 : 1;
        if (level == 20) return rand < 64f ? 3 : rand < 89f ? 2 : 1;
        if (level == 21) return rand < 63f ? 3 : rand < 88f ? 2 : 1;
        if (level == 22) return rand < 60f ? 3 : rand < 88f ? 2 : 1;
        if (level == 23) return rand < 58f ? 3 : rand < 88f ? 2 : 1;
        if (level == 24) return rand < 55f ? 3 : rand < 87f ? 2 : 1;
        if (level == 25) return rand < 50f ? 3 : rand < 85f ? 2 : 1;
        if (level == 26) return rand < 45f ? 3 : rand < 83f ? 2 : 1;
        if (level == 27) return rand < 60f ? 3 : rand < 80f ? 2 : 1;
        if (level == 28) return rand < 35f ? 3 : rand < 77f ? 2 : 1;
        if (level == 29) return rand < 30f ? 3 : rand < 75f ? 2 : 1;
        if (level == 30) return rand < 25f ? 3 : rand < 75f ? 2 : 1;

        return 3;
    }

    void GainExp(int fishType)
    {
        int size = fishType == 3 ? 0 : fishType == 2 ? 1 : 2;
        int expGained = fishType == 3 ? 1 : fishType == 2 ? 3 : 10;

        exp += expGained;

        int fishIndex = Random.Range(0, 12);
        int slotIndex = fishIndex * 3 + size;

        int currentCount = int.Parse(fishCountTexts[slotIndex].text);
        fishCountTexts[slotIndex].text = (++currentCount).ToString();

        while (exp >= maxExp) LevelUp();
        UpdateExpUI();
        SaveAllPlayerData();
        CheckBucketStatus();
    }

    void LevelUp()
    {
        if (level >= 30)
        {
            exp = maxExp;
            return;
        }

        exp -= maxExp;
        level++;

        switch (level)
        {
            case 2: maxExp = 25; break;
            case 3: maxExp = 50; break;
            case 4: maxExp = 80; break;
            case 5: maxExp = 115; break;
            case 6: maxExp = 150; break;
            case 7: maxExp = 200; break;
            case 8: maxExp = 255; break;
            case 9: maxExp = 320; break;
            case 10: maxExp = 400; break;
            case 11: maxExp = 500; break;
            case 12: maxExp = 610; break;
            case 13: maxExp = 750; break;
            case 14: maxExp = 1000; break;
            case 15: maxExp = 1500; break;
            case 16: maxExp = 2800; break;
            case 17: maxExp = 4200; break;
            case 18: maxExp = 5800; break;
            case 19: maxExp = 7000; break;
            case 20: maxExp = 9000; break;
            case 21: maxExp = 11000; break;
            case 22: maxExp = 13500; break;
            case 23: maxExp = 16500; break;
            case 24: maxExp = 20000; break;
            case 25: maxExp = 35000; break;
            case 26: maxExp = 30000; break;
            case 27: maxExp = 36000; break;
            case 28: maxExp = 43000; break;
            case 29: maxExp = 50000; break;
            case 30: maxExp = 0; break;
        }

        UpdateExpUI();
    }

    void UpdateExpUI()
    {
        expText.text = $"{exp} / {maxExp}";
        levelText.text = level.ToString();
        expSlider.value = (float)exp / maxExp;
    }

    void CheckBucketStatus()
    {
        bool hasAnyFish = false;

        foreach (Text fishText in fishCountTexts)
        {
            if (int.Parse(fishText.text) > 0)
            {
                hasAnyFish = true;
                break;
            }
        }

        bucketFullIndicator?.SetActive(hasAnyFish);
    }

    public void BuyFishingRod(int index, int price)
    {
        if (index < 0 || index >= rodNames.Length || index <= equippedRodIndex) return;

        if (gold >= price)
        {
            if (isFishing && fishingRoutine != null)
            {
                StopCoroutine(fishingRoutine);
                isFishing = false;
            }

            gold -= price;
            equippedRodIndex = index;
            UpdateRod();
            UpdateGoldUI();
            Debug.Log($"{rodNames[index]} 낚싯대 구매 완료!");
        }
        else
        {
            Debug.Log("골드 부족!");
        }
    }

    public void SellAllFish()
    {
        int totalGold = 0;

        for (int i = 0; i < fishCountTexts.Length; i++)
        {
            int count = int.Parse(fishCountTexts[i].text);
            if (count <= 0) continue;

            int price = i % 3 == 0 ? 50 : i % 3 == 1 ? 100 : 300;
            totalGold += price * count;

            fishCountTexts[i].text = "0";
        }

        gold += totalGold;
        UpdateGoldUI();

        if (coinAudioSource != null && totalGold > 0)
            coinAudioSource.Play();

        SaveAllPlayerData();
        Debug.Log($"모든 물고기 판매 완료! +{totalGold} 골드");
    }

    public void SellFishByButtonIndex(int index)
    {
        Text countText = fishCountTexts[index];
        int count = int.Parse(countText.text);
        if (count <= 0) return;

        int price = index % 3 == 0 ? 50 : index % 3 == 1 ? 100 : 300;

        gold += price;

        if (coinAudioSource != null)
            coinAudioSource.Play();

        countText.text = (count - 1).ToString();
        UpdateGoldUI();

        Debug.Log($"[{index + 1}번 슬롯 판매] +{price} 골드!");

        SaveAllPlayerData();
        CheckBucketStatus();
    }

    public void SaveAllPlayerData()
    {
        int[] fishCounts = new int[fishCountTexts.Length];
        for (int i = 0; i < fishCountTexts.Length; i++)
            fishCounts[i] = int.Parse(fishCountTexts[i].text);

        SaveManager.SavePlayerData(level, exp, gold, equippedRodIndex, fishCounts);
    }
}
