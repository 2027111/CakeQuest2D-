using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenuButton : ChoiceMenuButton
{
    [SerializeField] string indicationText;
    public override void OnOver(bool isOverHanded)
    {
        base.OnOver(isOverHanded);
        if (isOverHanded)
        {
            string t = LanguageData.GetDataById("Indications").GetValueByKey(indicationText);
            BattleManager.Singleton.SetIndicationText(t);
        }
    }
}
