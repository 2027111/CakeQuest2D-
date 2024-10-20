using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI usernametext;
    BattleCharacter characterReference;
    [SerializeField] Image portraitImage;
    [SerializeField] HealthBar healthBar;
    [SerializeField] HealthBar manaBar;

    public void SetPlayerRef(BattleCharacter character)
    {
        characterReference = character;
        if (characterReference.GetData().portraits.Length > 0)
        {
            portraitImage.gameObject.SetActive(true);
            portraitImage.sprite = characterReference.GetData().portraits[0];
        }
        else
        {
            portraitImage.gameObject.SetActive(false);
        }
        healthBar?.SetBarName(characterReference.GetData().HealthName);
        manaBar?.SetBarName(characterReference.GetData().ManaName);
        usernametext?.SetText($"{characterReference.GetData().characterName}");

        healthBar?.SetFillAmount(characterReference.Entity.Health, characterReference.GetReference().MaxHealth);
        manaBar?.SetFillAmount(characterReference.Entity.Mana, characterReference.GetReference().MaxMana);
    }





}
