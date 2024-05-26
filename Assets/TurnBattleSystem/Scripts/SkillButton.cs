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
    Vector2 baseSize = new Vector2(160, 40);

    private void Start()
    {

    }
    public void SetSkill(Skill attack)
    {
        storedSkill = attack;


        attackNameText.text = storedSkill.Name;
        attackDescText.text = "Type : " + storedSkill.element.ToString() + " | " + storedSkill.Description + " | Does " + storedSkill.baseDamage + " damage points every hit";
        attackCostText.text = storedSkill.manaCost.ToString();
        Sprite attackSprite = Resources.Load<Sprite>(logoPath + storedSkill.element.ToString().ToLower());
        if (attackSprite)
        {
            attackTypeLogo.sprite = attackSprite;
        }
    }

    public override void OnOver(bool isOverHanded)
    {
        RectTransform trans = transform.GetComponent<RectTransform>();
        Vector2 vec = baseSize;
        if (isOverHanded)
        {
            vec = new Vector2(baseSize.x, baseSize.y * 2f);
        }
        trans.sizeDelta = vec;

    }


}

