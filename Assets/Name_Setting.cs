using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Name_Setting : MonoBehaviour
{
    public TMP_InputField inputField;

    void Start()
    {
        inputField.characterLimit = 8;
    }
}
