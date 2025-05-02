using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public Button startButton;          // 시작 버튼
    public Button nameConfirmButton;    // 이름 확인 버튼

    public GameObject Black_Background;
    public GameObject NameSettingPannel;
    public GameObject WarningText;

    public TMP_InputField inputField;

    void Start()
    {
        startButton.onClick.AddListener(ToggleStart);
        nameConfirmButton.onClick.AddListener(CheckNameValid);

        Black_Background.SetActive(false);
        startButton.gameObject.SetActive(true);
        NameSettingPannel.SetActive(false);
        WarningText.SetActive(false);
        inputField.characterLimit = 8;

        // 저장된 이름이 있다면 불러오기
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            string savedName = PlayerPrefs.GetString("PlayerName");
            inputField.text = savedName;
            Debug.Log("저장된 이름 불러옴: " + savedName);
        }
    }

    void ToggleStart()
    {
        // 저장된 이름이 있다면 바로 씬 이동
        if (PlayerPrefs.HasKey("PlayerName") && PlayerPrefs.GetString("PlayerName").Length > 1)
        {
            SceneManager.LoadScene("Korea_Summer");
        }
        else
        {
            startButton.gameObject.SetActive(false);
            StartCoroutine(NameSetting());
        }
    }

    IEnumerator NameSetting()
    {
        Black_Background.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        NameSettingPannel.SetActive(true);
    }

    void CheckNameValid()
    {
        if (inputField.text.Length <= 1)
        {
            WarningText.SetActive(true);
            return;
        }

        // 이름 저장 후 씬 이동
        PlayerPrefs.SetString("PlayerName", inputField.text);
        PlayerPrefs.Save();
        Debug.Log("이름 저장됨: " + inputField.text);

        SceneManager.LoadScene("Korea_Summer");
    }
}
