using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI usernametext;
    CharacterObject characterReference;
    [SerializeField] Image portraitImage;
    [SerializeField] HealthBar healthBar;
    [SerializeField] HealthBar manaBar;

    public void SetPlayerRef(CharacterObject character)
    {
        characterReference = character;
        //characterReference.OnHealthChange += healthBar.SetFillAmount;
        //characterReference.OnManaChange += manaBar.SetFillAmount;
        if (characterReference.characterData.portraits.Length > 0)
        {
            portraitImage.gameObject.SetActive(true);
            portraitImage.sprite = characterReference.characterData.portraits[0];
        }
        else
        {
            portraitImage.gameObject.SetActive(false);
        }
        healthBar.SetBarName(characterReference.characterData.HealthName);
        manaBar.SetBarName(characterReference.characterData.ManaName);
        usernametext.text = $"{characterReference.characterData.characterName}";



        //characterReference.OnHealthChange.Invoke(characterReference.Health, characterReference.MaxHealth);
        //characterReference.OnManaChange.Invoke(characterReference.Mana, characterReference.MaxMana);


    }



    private void OnDisable()
    {
        //characterReference.OnHealthChange -= healthBar.SetFillAmount;
        //characterReference.OnManaChange -= manaBar.SetFillAmount;
    }


    
}
