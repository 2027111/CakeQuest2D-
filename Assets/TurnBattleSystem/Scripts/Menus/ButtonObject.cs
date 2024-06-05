using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonObject : MonoBehaviour
{

    [SerializeField] TMP_Text text;
    public void SetText(string newText)
    {
        text.text = newText;
    }


    public string GetText()
    {
        return text.text;
    }
}
