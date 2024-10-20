using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PartyCard : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI usernametext;
    BattleCharacter characterReference;
    [SerializeField] Image portraitImage;
    [SerializeField] HealthBar healthBar;
    [SerializeField] HealthBar manaBar;
    [SerializeField] HealthBar focusBar;

    public void SetPlayerRef(BattleCharacter character)
    {
        characterReference = character;
        characterReference.Entity.OnHealthChange += healthBar.SetFillAmount;
        characterReference.Entity.OnManaChange += manaBar.SetFillAmount;
        characterReference.Entity.OnFocusChange += focusBar.SetFillAmount;
        if (characterReference.GetData().portraits.Length > 0)
        {
            portraitImage.gameObject.SetActive(true);
            portraitImage.sprite = characterReference.GetData().portraits[0];
        }
        else
        {
            portraitImage.gameObject.SetActive(false);
        }
        //usernametext.text = $"{characterReference.characterData.characterName}";



        characterReference.Entity.AddToHealth(0);
        characterReference.Entity.AddToMana(0);


    }



    private void OnDisable()
    {
        characterReference.Entity.OnHealthChange -= healthBar.SetFillAmount;
        characterReference.Entity.OnManaChange -= manaBar.SetFillAmount;
        characterReference.Entity.OnFocusChange -= focusBar.SetFillAmount;
    }



}
