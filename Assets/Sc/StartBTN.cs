using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartBTN : MonoBehaviour
{
    public Button startButton;
    public GameObject Black_Background;

    void Start()
    {
        startButton.onClick.AddListener(ToggleStart);
        Black_Background.SetActive(false);
        startButton.gameObject.SetActive(true);
    }

    void ToggleStart()
    {
        Black_Background.SetActive(true);
        startButton.gameObject.SetActive(false);
        // SceneManager.LoadScene("Korea_Summer");
    }

    void Update()
    {

    }
}
