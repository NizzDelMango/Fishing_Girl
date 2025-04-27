using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System.Collections;

public class InterstitialAdManager : MonoBehaviour
{
    private InterstitialAd interstitialAd;
    private string adUnitId = "ca-app-pub-3940256099942544/1033173712"; // 테스트용 ID

    void Start()
    {
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Google Mobile Ads SDK 초기화 완료");
            LoadInterstitialAd();
            StartCoroutine(ShowAdAfterDelay());
        });
    }

    public void LoadInterstitialAd()
    {
        // 이전 광고 객체 제거
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        var adRequest = new AdRequest();

        InterstitialAd.Load(adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError($"전면 광고 로드 실패: {error}");
                return;
            }

            Debug.Log("전면 광고 로드 성공");
            interstitialAd = ad;

            // 광고 닫힐 때 이벤트 연결
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("전면 광고 닫힘");
                LoadInterstitialAd(); // 닫히면 새로 로드
            };
        });
    }

    IEnumerator ShowAdAfterDelay()
    {
        yield return new WaitForSeconds(5f); // 5초 대기

        ShowInterstitialAd();
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("전면 광고가 아직 준비 안 됐음");
        }
    }

    private void OnDestroy()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }
}
