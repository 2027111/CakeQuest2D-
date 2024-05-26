using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : ChoiceMenuButton
{
    [SerializeField] string logoPath = "Sprites/logo_element_";
    public Skill storedSkill;

    [SerializeField] TMP_Text attackNameText;
    [SerializeField] TMP_Text attackDescText;
    [SerializeField] TMP_Text attackCostText;
    [SerializeField] Image attackTypeLogo;

    private void Start()
    {

    }
    public void SetSkill(Skill attack)
    {
        storedSkill = attack;


        attackNameText.text = storedSkill.Name;
        attackCostText.text = storedSkill.manaCost.ToString();
        Sprite attackSprite = Resources.Load<Sprite>(logoPath + storedSkill.element.ToString().ToLower());
        if (attackSprite)
        {
            attackTypeLogo.sprite = attackSprite;
        }
    }

    public override void OnOver(bool isOverHanded)
    {
        if (isOverHanded)
        {
            BattleManager.Singleton.SetIndicationText("Type : " + storedSkill.element.ToString() + " | " + storedSkill.Description + " | Does " + storedSkill.baseDamage + " damage points every hit");

            //string t = LanguageData.GetDataById("Indications").GetValueByKey(indicationText);
            //BattleManager.Singleton.SetIndicationText(t);
        }
        
    }


}

