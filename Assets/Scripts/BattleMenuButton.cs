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
            BattleManager.Singleton.SetIndicationText(indicationText);
        }
    }
}
