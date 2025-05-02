using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public Button startButton;          // ���� ��ư
    public Button nameConfirmButton;    // �̸� Ȯ�� ��ư

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

        // ����� �̸��� �ִٸ� �ҷ�����
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            string savedName = PlayerPrefs.GetString("PlayerName");
            inputField.text = savedName;
            Debug.Log("����� �̸� �ҷ���: " + savedName);
        }
    }

    void ToggleStart()
    {
        // ����� �̸��� �ִٸ� �ٷ� �� �̵�
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

        // �̸� ���� �� �� �̵�
        PlayerPrefs.SetString("PlayerName", inputField.text);
        PlayerPrefs.Save();
        Debug.Log("�̸� �����: " + inputField.text);

        SceneManager.LoadScene("Korea_Summer");
    }
}
