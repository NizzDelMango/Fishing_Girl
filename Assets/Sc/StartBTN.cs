using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartBTN : MonoBehaviour
{
    public Button startButton;
    void Start()
    {
        startButton.onClick.AddListener(ToggleStart);
    }

    void ToggleStart()
    {
        SceneManager.LoadScene("InGameScene");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
