using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoSetup : MonoBehaviour
{

    public string spriteSheetName;
    public int spriteIndex;

    private Sprite[] sprites;


    void LoadSpriteSheet()
    {
        // Load all sprites from the sprite sheet
        sprites = Resources.LoadAll<Sprite>(spriteSheetName);

        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError("Failed to load sprite sheet: " + spriteSheetName);
        }
    }

    Sprite GetSpriteByIndex(int index)
    {
        if (sprites != null && index >= 0 && index < sprites.Length)
        {
            return sprites[index];
        }
        return null;
    }

    [SerializeField] TMP_Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        GameSaveManager.Singleton?.OnLanguageChanged.AddListener(SetDropDownValue);

        LoadSpriteSheet();
        SetDropDownValue();
    }



    public void SetDropDownValue()
    {
            dropdown.SetValueWithoutNotify((int)LanguageData.language);
            for(int i = 0; i < dropdown.options.Count;i++)
            {
                if(i < sprites.Length)
                {
                    dropdown.options[i].image = GetSpriteByIndex(i);
                }
            }
    }

    public void OnLanguageChanges(int option)
    {
        GameSaveManager.Singleton?.OnLanguageDropdownValueChanged(option);
    }

}
