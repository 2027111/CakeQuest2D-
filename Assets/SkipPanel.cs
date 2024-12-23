using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipPanel : MonoBehaviour
{
    [SerializeField] Image fillBar;
    [SerializeField] CanvasGroup canvasGroup;

    public void SetSkipPanel(float ratio)
    {
        if (ratio > 0)
        {
            if (canvasGroup.alpha == 0) { canvasGroup.alpha = 1; }
            fillBar.fillAmount = ratio;

        }
        else
        {
            canvasGroup.alpha = 0;
            fillBar.fillAmount = ratio;
        }

    }
}
