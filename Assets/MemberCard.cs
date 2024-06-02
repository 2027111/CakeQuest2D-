using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemberCard : MonoBehaviour
{

    [SerializeField] HealthBar healthBar;
    [SerializeField] Image portrait;
    [SerializeField] CharacterObject characterObject;


    public void SetCharObject(CharacterObject characterObject)
    {
        this.characterObject = characterObject;
        portrait.sprite = this.characterObject.characterData.GetPortrait();
        UpdateHealthBar(this.characterObject.Health, this.characterObject.MaxHealth);
    }


    public void UpdateHealthBar(int amount, int maxAmount)
    {
        healthBar.SetFillAmount(amount, maxAmount);
    }

    public void UpdateHealthBar()
    {
        healthBar.SetFillAmount(this.characterObject.Health, this.characterObject.MaxHealth);
    }
}
