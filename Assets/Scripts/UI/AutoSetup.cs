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
    }
    public void LoadSpriteSheet()
    {
        // Load all sprites from the sprite sheet
        sprites = Resources.LoadAll<Sprite>(spriteSheetName);

        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError("Failed to load sprite sheet: " + spriteSheetName);
        }

        InitDropdown();
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


            if (i < sprites.Length)
            {
                dropdownOptions[i].image = GetSpriteByIndex(i);
            }

        }

        // Add the options to the dropdown
        dropdown.AddOptions(new System.Collections.Generic.List<TMP_Dropdown.OptionData>(dropdownOptions));

        //languageDropdown.onValueChanged.AddListener(GameSaveManager.Singleton.OnLanguageDropdownValueChanged);
        int initialValue = 0;
        if (LanguageData.Loaded())
        {
            initialValue = (int)LanguageData.GetLanguage();
        }
        else
        {
            initialValue = (int)GamePreference.Language;
        }
        dropdown.SetValueWithoutNotify(initialValue);
        UpdateCaptionImage(initialValue);
    }

    private void UpdateCaptionImage(int selectedIndex)
    {
        // Update the caption image based on the selected option
        if (dropdown.captionImage != null && selectedIndex >= 0 && selectedIndex < sprites.Length)
        {
            dropdown.captionImage.sprite = GetSpriteByIndex(selectedIndex);
        }
    }

    public void SetDropDownValue()
    {
        int value = (int)LanguageData.language;
        dropdown.SetValueWithoutNotify(value);
        UpdateCaptionImage(value);
    }

    public void OnLanguageChanges(int option)
    {
        GameSaveManager.Singleton?.OnLanguageDropdownValueChanged(option);
    }

}
