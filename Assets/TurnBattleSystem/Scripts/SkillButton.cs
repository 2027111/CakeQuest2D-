using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : ChoiceMenuButton
{

    public Attack storedSkill;
    public void SetSkill(Attack attack)
    {
        storedSkill = attack;


        attackNameText.text = storedSkill.Name;
        attackCostText.text = storedSkill.manaCost.ToString();
        Sprite attackSprite = Resources.Load<Sprite>("Sprites/attackLogo_" + storedSkill.skillType);
        if (attackSprite)
        {
            attackTypeLogo.sprite = attackSprite;
        }
    }

    [SerializeField] TMP_Text attackNameText;
    [SerializeField] TMP_Text attackCostText;
    [SerializeField] Image attackTypeLogo;


}

