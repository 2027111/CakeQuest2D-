using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image fillBar;
    [SerializeField] Image underfillBar;
    [SerializeField] TextMeshProUGUI bartext;
    [SerializeField] TextMeshProUGUI barNameText;
    [SerializeField] float delayTime = 1;
    [SerializeField] float catchUpTime = 1;
    bool underfilling = false;



    public void SetBarName(string name)
    {
        barNameText.text = name;
    }
    IEnumerator DelayUnderFill()
    {
        underfilling = true;
        float startFill = underfillBar.fillAmount;
        yield return new WaitForSeconds(delayTime);

        float time = 0f;
        float targetFillAmount = fillBar.fillAmount;

        while (underfillBar.fillAmount > fillBar.fillAmount)
        {
            float newFill = Mathf.Lerp(startFill, targetFillAmount, time);
            underfillBar.fillAmount = newFill;
            time += Time.deltaTime * catchUpTime; // Use Time.deltaTime for smoother interpolation
            yield return null;
        }

        underfillBar.fillAmount = targetFillAmount; // Ensure underfillBar matches fillBar
        underfilling = false;
    }

    public void SetFillAmount(int currentHealth, int maxHealth)
    {
        fillBar.fillAmount = (float)((float)currentHealth / (float)maxHealth);
        bartext?.SetText(currentHealth.ToString());
        if (!underfilling)
        {
            StartCoroutine(DelayUnderFill());
        }

    }
}
