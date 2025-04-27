using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System.Collections;

public class InterstitialAdManager : MonoBehaviour
{
    private InterstitialAd interstitialAd;
    private string adUnitId = "ca-app-pub-3940256099942544/1033173712"; // �׽�Ʈ�� ID

    void Start()
    {
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Google Mobile Ads SDK �ʱ�ȭ �Ϸ�");
            LoadInterstitialAd();
            StartCoroutine(ShowAdAfterDelay());
        });
    }

    public void LoadInterstitialAd()
    {
        // ���� ���� ��ü ����
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
                Debug.LogError($"���� ���� �ε� ����: {error}");
                return;
            }

            Debug.Log("���� ���� �ε� ����");
            interstitialAd = ad;

            // ���� ���� �� �̺�Ʈ ����
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("���� ���� ����");
                LoadInterstitialAd(); // ������ ���� �ε�
            };
        });
    }

    IEnumerator ShowAdAfterDelay()
    {
        yield return new WaitForSeconds(5f); // 5�� ���

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
            Debug.Log("���� ���� ���� �غ� �� ����");
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
