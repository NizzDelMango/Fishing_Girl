using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Manager : MonoBehaviour
{
    [Header("패널 오브젝트")]
    public GameObject Guide_Panel;
    public GameObject Settings_Panel;
    public GameObject Main_Panel;

    [Header("게임 매니저 참조")]
    public GameManager gamemanager;

    void Update()
    {
        // ESC 또는 안드로이드 뒤로가기 버튼 입력 감지
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleBackAction();
        }
    }

    void HandleBackAction()
    {
        // 만약 Main 패널이 켜져 있으면 전체 메뉴를 끔
        if (Main_Panel.activeSelf)
        {
            this.gameObject.SetActive(false);
        }
        // 그 외에는 Guide, Settings 끄고 Main 켬
        else
        {
            Guide_Panel.SetActive(false);
            Settings_Panel.SetActive(false);
            Main_Panel.SetActive(true);
        }
    }

    // 메뉴 열기
    public void OpenMenu()
    {
        this.gameObject.SetActive(true);
        Guide_Panel.SetActive(false);
        Settings_Panel.SetActive(false);
        Main_Panel.SetActive(true);
    }

    public void OpenGuide()
    {
        Guide_Panel.SetActive(true);
        Settings_Panel.SetActive(false);
        Main_Panel.SetActive(false);
    }

    public void OpenSettings()
    {
        Settings_Panel.SetActive(true);
        Guide_Panel.SetActive(false);
        Main_Panel.SetActive(false);
    }

    public void SaveAndExit()
    {
        if (gamemanager != null)
        {
            gamemanager.SavePlayerData();
            Debug.Log("Player data saved successfully.");
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
