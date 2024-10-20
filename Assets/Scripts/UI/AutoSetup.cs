using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoSetup : MonoBehaviour
{

    public string spriteSheetName;
    public int spriteIndex;
    private Sprite[] sprites;
    [SerializeField] TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();

        LoadSpriteSheet();
        InitDropdown();
    }
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

    // Start is called before the first frame update
    void Start()
    {
        GameSaveManager.Singleton?.OnLanguageChanged.AddListener(SetDropDownValue);

    }


    private void InitDropdown()
    {
        // Clear existing options
        dropdown.ClearOptions();

        // Get the names of the enum values
        string[] languageNames = System.Enum.GetNames(typeof(Language));

        // Convert the enum names to dropdown options
        TMP_Dropdown.OptionData[] dropdownOptions = new TMP_Dropdown.OptionData[languageNames.Length];
        for (int i = 0; i < languageNames.Length; i++)
        {
            dropdownOptions[i] = new TMP_Dropdown.OptionData(languageNames[i]);

        }

        // Add the options to the dropdown
        dropdown.AddOptions(new System.Collections.Generic.List<TMP_Dropdown.OptionData>(dropdownOptions));

        //languageDropdown.onValueChanged.AddListener(GameSaveManager.Singleton.OnLanguageDropdownValueChanged);
    }

    public void SetDropDownValue()
    {
        dropdown.SetValueWithoutNotify((int)LanguageData.language);
        for (int i = 0; i < dropdown.options.Count; i++)
        {

            if (i < sprites.Length)
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
