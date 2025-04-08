using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player_Stats : MonoBehaviour
{
    public Animator characterAnimator;
    public Text expText, levelText;
    public Slider expSlider;
    public Text[] fishCountTexts = new Text[36];
    public Text goldText;

    [Range(0, 3)]
    public int equippedRodIndex = 0; // 0 = Bamboo, 1 = Old, 2 = Iron, 3 = Gold
    public int gold = 100000;

    private int level = 1, exp = 0, maxExp = 10;
    private bool isFishing = false;
    private Coroutine fishingRoutine;
    private int previousRodIndex = -1; // 추가: 낚싯대 변경 감지용

    private readonly string[] rodNames = {
        "Bamboo_fishing_rod",
        "Old_fishing_rod",
        "Iron_fishing_rod",
        "Master_fishing_rod"
    };

    private float[] rodTimes = { 10f, 9f, 7f, 5f };

    void Start()
    {
        UpdateRod();
        UpdateExpUI();
        UpdateGoldUI();
    }

    void Update()
    {
        UpdateRod(); // 낚싯대 변경 감지

        if (characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fishing"))
            characterAnimator.SetInteger("Fish", 0);

        if (!isFishing && (Input.GetMouseButtonDown(0) || Input.touchCount > 0))
            fishingRoutine = StartCoroutine(FishingProcess());
    }

    void UpdateGoldUI()
    {
        goldText.text = gold.ToString();
    }

    void UpdateRod()
    {
        equippedRodIndex = Mathf.Clamp(equippedRodIndex, 0, rodNames.Length - 1);
        characterAnimator.SetInteger("Fishing_rod", equippedRodIndex);

        // 낚싯대 변경 시 낚시 중단
        if (previousRodIndex != equippedRodIndex)
        {
            if (isFishing && fishingRoutine != null)
            {
                StopCoroutine(fishingRoutine);
                isFishing = false;
            }

            previousRodIndex = equippedRodIndex;
        }

        // Animator 레이어 가중치 조절
        for (int i = 1; i <= 4; i++)
        {
            float weight = (i - 1 == equippedRodIndex) ? 1f : 0f;
            characterAnimator.SetLayerWeight(i, weight);
        }
    }

    float GetRodAnimationTime() => rodTimes[equippedRodIndex];

    IEnumerator FishingProcess()
    {
        isFishing = true;
        characterAnimator.SetTrigger("Touched");
        characterAnimator.SetInteger("Fish", 0);

        float time = GetRodAnimationTime();
        float elapsed = 0f;

        // 낚시 진행 시간 체크
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
        if (level <= 9) return rand < 97 ? 3 : rand < 99 ? 2 : 1;
        if (level <= 13) return rand < 90 ? 3 : rand < 98 ? 2 : 1;
        if (level <= 16) return rand < 80 ? 3 : rand < 95 ? 2 : 1;
        if (level <= 20) return rand < 70 ? 3 : rand < 95 ? 2 : 1;
        if (level <= 25) return rand < 50 ? 3 : rand < 85 ? 2 : 1;
        if (level <= 28) return rand < 40 ? 3 : rand < 80 ? 2 : 1;
        return rand < 25 ? 3 : rand < 75 ? 2 : 1;
    }

    void GainExp(int fishType)
    {
        int size;
        int expGained;

        if (fishType == 3) { size = 0; expGained = 1; }
        else if (fishType == 2) { size = 1; expGained = 3; }
        else { size = 2; expGained = 10; }

        exp += expGained;

        int randomFishIndex = Random.Range(0, 12);
        int textIndex = randomFishIndex * 3 + size;

        int currentCount = int.Parse(fishCountTexts[textIndex].text);
        currentCount++;
        fishCountTexts[textIndex].text = currentCount.ToString();

        while (exp >= maxExp) LevelUp();
        UpdateExpUI();
    }

    void LevelUp()
    {
        exp -= maxExp;
        level++;

        if (level <= 6) maxExp = (int)(25 * Mathf.Pow(1.5f, level - 2));
        else if (level <= 10) maxExp = 150 + (level - 6) * 50;
        else maxExp += 5000;

        UpdateExpUI();
    }

    void UpdateExpUI()
    {
        expText.text = exp + " / " + maxExp;
        levelText.text = level.ToString();
        expSlider.value = (float)exp / maxExp;
    }

    public void BuyFishingRod(int index, int price)
    {
        if (index < 0 || index >= rodNames.Length) return;

        if (index < equippedRodIndex)
        {
            Debug.Log("더 낮은 등급의 낚싯대는 다시 구매할 수 없습니다!");
            return;
        }

        if (index == equippedRodIndex)
        {
            Debug.Log("이미 이 낚싯대를 사용 중입니다!");
            return;
        }

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
            Debug.Log($"{rodNames[index]} 낚싯대를 구매했습니다. 남은 골드: {gold}");
        }
        else
        {
            Debug.Log("골드가 부족합니다!");
        }
    }

    public void SellFishByButtonIndex(int index)
    {
        Debug.Log($"판매 버튼 클릭됨! index = {index}");

        Text countText = fishCountTexts[index];
        int count = int.Parse(countText.text);

        if (count <= 0) return;

        int price = 0;
        if (index < 12) price = 50;
        else if (index < 24) price = 100;
        else price = 300;

        gold += price;
        countText.text = (count - 1).ToString();
        UpdateGoldUI();
        Debug.Log($"[{index + 1}번 슬롯 판매] +{price}원! 현재 골드: {gold}");
    }
}
