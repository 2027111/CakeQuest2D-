using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIToggleIndicatorText : MonoBehaviour
{

    [SerializeField] TMP_Text indicatorText;
    public string ToggledOnKey;
    public string ToggledOffKey;

    public void Toggle(bool toggled)
    {
        indicatorText?.SetText(LanguageData.GetDataById(LanguageData.INDICATION).GetValueByKey(toggled ? ToggledOnKey : ToggledOffKey));
    }

}
