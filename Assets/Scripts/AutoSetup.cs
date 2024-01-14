using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoSetup : MonoBehaviour
{



    [SerializeField] TMP_Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        GameSaveManager.Singleton?.OnLanguageChanged.AddListener(SetDropDownValue);
        SetDropDownValue();
    }



    public void SetDropDownValue()
    {
            dropdown.SetValueWithoutNotify((int)GameSaveManager.Singleton?.currentLanguage.language);
        
    }

    public void OnLanguageChanges(int option)
    {
        GameSaveManager.Singleton?.OnLanguageDropdownValueChanged(option);
    }

}
