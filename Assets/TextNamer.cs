using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextNamer : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text.text = gameObject.name;
    }
}
