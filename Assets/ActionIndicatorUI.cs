using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionIndicatorUI : MonoBehaviour
{
    public Image IndicatorBackgroundImage;
    public TMP_Text indicationTextField;
    public TMP_Text controlIcon;



    public void AppearIndicator(bool showAction, string indication = "interact")
    {

        IndicatorBackgroundImage.gameObject.SetActive(showAction);

        if (!showAction)
        {
            return;
        }

        string indicationText = LanguageData.GetDataById(LanguageData.INDICATION).GetValueByKey(indication);
        indicationTextField.SetText(indicationText);
    }
}
