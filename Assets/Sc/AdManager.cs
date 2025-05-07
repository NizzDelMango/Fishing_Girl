using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance; // 다른 스크립트에서 접근할 수 있게 싱글톤 사용
    private RewardedAd rewardedAd;
    private string adUnitId = "ca-app-pub-3940256099942544/5224354917"; // 테스트 광고 ID

    private Action onAdSuccess; // 광고 성공 후 실행할 콜백

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        MobileAds.Initialize(initStatus => {
            LoadRewardedAd();
        });
    }

    public void LoadRewardedAd()
    {
        AdRequest request = new AdRequest();

        RewardedAd.Load(adUnitId, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("광고 로드 실패: " + error?.GetMessage());
                return;
            }

            rewardedAd = ad;

            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("광고 닫힘");
                LoadRewardedAd();
            };

            rewardedAd.OnAdFullScreenContentFailed += (AdError adError) =>
            {
                Debug.LogError("광고 실패: " + adError.GetMessage());
            };
        });
    }

    public void ShowRewardedAd(Action rewardCallback)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            onAdSuccess = rewardCallback;

            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("광고 보상 완료");
                onAdSuccess?.Invoke(); // 낚싯대 지급
            });
        }
        else
        {
            Debug.Log("광고 없음");
        }
    }
}