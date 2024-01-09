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
        characterReference.OnHealthChange += healthBar.SetFillAmount;
        characterReference.OnManaChange += manaBar.SetFillAmount;
        portraitImage.sprite = characterReference.portraits[0];
        healthBar.SetBarName(characterReference.HealthName);
        manaBar.SetBarName(characterReference.ManaName);
        usernametext.text = $"{characterReference.characterName}";



        characterReference.OnHealthChange.Invoke(characterReference.Health, characterReference.MaxHealth);
        characterReference.OnManaChange.Invoke(characterReference.Mana, characterReference.MaxMana);


    }



    private void OnDisable()
    {
        characterReference.OnHealthChange -= healthBar.SetFillAmount;
        characterReference.OnManaChange -= manaBar.SetFillAmount;
    }


    
}
