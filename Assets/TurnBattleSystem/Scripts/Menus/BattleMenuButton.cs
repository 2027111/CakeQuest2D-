using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMenuButton : ChoiceMenuButton
{
    [SerializeField] string indicationText;

    private void Start()
    {
        if (disabled)
        {
            GetComponent<Image>().color = new Color(.3f, .3f, .3f, .3f);
        }
        else
        {
            GetComponent<Image>().color = new Color(1, 1, 1, 1);

        }
    }
    public override void OnOver(bool isOverHanded)
    {
        base.OnOver(isOverHanded);
        if (isOverHanded)
        {
            string t = LanguageData.GetDataById(LanguageData.INDICATION).GetValueByKey(indicationText);
            BattleManager.Singleton.SetIndicationText(t);
        }
    }
}
