using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_Manager : MonoBehaviour
{
    [Header("상점 패널")]
    public GameObject Rod_Panel;
    public GameObject Clothes_Panel;
    public GameObject Country_Panel;

    void Start()
    {
        // 초기에는 비활성화
        this.gameObject.SetActive(false);
        Rod_Panel.SetActive(true);
        Clothes_Panel.SetActive(false);
        Country_Panel.SetActive(false);
    }

    public void showShop()
    {
        // 상점 토글
        bool isActive = this.gameObject.activeSelf;
        this.gameObject.SetActive(!isActive);

        // 켜질 때 기본 탭 설정
        if (!isActive)
        {
            Rod_Panel.SetActive(true);
            Clothes_Panel.SetActive(false);
            Country_Panel.SetActive(false);
        }
    }

    public void showRodPanel()
    {
        Rod_Panel.SetActive(true);
        Clothes_Panel.SetActive(false);
        Country_Panel.SetActive(false);
    }

    public void showClothesPanel()
    {
        Rod_Panel.SetActive(false);
        Clothes_Panel.SetActive(true);
        Country_Panel.SetActive(false);
    }

    public void showCountryPanel()
    {
        Rod_Panel.SetActive(false);
        Clothes_Panel.SetActive(false);
        Country_Panel.SetActive(true);
    }
}
