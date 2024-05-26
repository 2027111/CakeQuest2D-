using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextNamer : MonoBehaviour
{
    [SerializeField] TMP_Text NameText;
    [SerializeField] TMP_Text StateText;
    // Start is called before the first frame update
    void Start()
    {
        NameText.text = gameObject.name;
    }

    private void Update()
    {
        StateText.text = GetComponent<Character>().GetCurrentBehaviour().GetType().ToString();
    }
}
