using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player_Stats : MonoBehaviour
{
    public Animator characterAnimator;
    public Text expText, levelText;
    public Slider expSlider;
    public Text[] fishCountTexts = new Text[36];
    public Text goldText; // 현재 골드 표시용 Text


    [Range(0, 3)]
    public int equippedRodIndex = 0; // 인스펙터에서 0~3 사이 숫자 선택
    // Bamboo_fishing_rod = 0
    // Old_fishing_rod = 1
    // Iron_fishing_rod = 2
    // Gold_fishing_rod = 3

    public int gold = 100000;

    private int level = 1, exp = 0, maxExp = 10;
    private bool isFishing = false;

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
        // 낚싯대 인덱스 변경 시 자동 반영
        UpdateRod();

        if (characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fishing"))
            characterAnimator.SetInteger("Fish", 0);

        if (!isFishing && (Input.GetMouseButtonDown(0) || Input.touchCount > 0))
            StartCoroutine(FishingProcess());
    }

    void UpdateGoldUI()
    {
        goldText.text = gold.ToString(); // 골드를 텍스트로 표시
    }

    void UpdateRod()
    {
        equippedRodIndex = Mathf.Clamp(equippedRodIndex, 0, rodNames.Length - 1);
        characterAnimator.SetInteger("Fishing_rod", equippedRodIndex); // 애니메이터 파라미터 업데이트
    }

    float GetRodAnimationTime() => rodTimes[equippedRodIndex];

    IEnumerator FishingProcess()
    {
        isFishing = true;
        characterAnimator.SetTrigger("Touched");
        characterAnimator.SetInteger("Fish", 0);

        yield return new WaitForSeconds(GetRodAnimationTime());

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

        if (fishType == 3) { size = 0; expGained = 1; }      // 소형
        else if (fishType == 2) { size = 1; expGained = 3; } // 중형
        else { size = 2; expGained = 10; }                   // 대형

        exp += expGained;

        // 0~11번 사이에서 물고기 종류 하나 랜덤 선택
        int randomFishIndex = Random.Range(0, 12);

        // 텍스트 인덱스 계산: 종류 * 3 + 크기
        int textIndex = randomFishIndex * 3 + size;

        //  현재 텍스트 숫자를 읽고 +1 하기
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

        // 이미 더 좋은 낚싯대를 샀다면 낮은 등급은 구매 불가
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
        if (index < 12) price = 50;       // 소형
        else if (index < 24) price = 100; // 중형
        else price = 300;                 // 대형

        gold += price;
        countText.text = (count - 1).ToString();
        UpdateGoldUI();
        Debug.Log($"[{index + 1}번 슬롯 판매] +{price}원! 현재 골드: {gold}");
    }


}