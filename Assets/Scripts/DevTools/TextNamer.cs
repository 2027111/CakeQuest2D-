using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextNamer : MonoBehaviour
{
    [SerializeField] TMP_Text NameText;
    [SerializeField] TMP_Text StateText;
    Character thisChara;
    // Start is called before the first frame update
    void Start()
    {
        thisChara = GetComponentInParent<Character>();


        NameText.text = thisChara.name;
    }

    private void Update()
    {
        StateText.text = thisChara.GetCurrentBehaviour().ToString();
    }
}
