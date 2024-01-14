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
    bool underfilling = false;



    public void SetBarName(string name)
    {
        barNameText.text = name;
    }
    IEnumerator DelayUnderFill()
    {
        underfilling = true;
        float startFill = underfillBar.fillAmount;
        yield return new WaitForSeconds(1f);

        float time = 0f;
        float targetFillAmount = fillBar.fillAmount;

        while (underfillBar.fillAmount > targetFillAmount)
        {
            float newFill = Mathf.Lerp(startFill, targetFillAmount, time);
            underfillBar.fillAmount = newFill;
            time += Time.deltaTime; // Use Time.deltaTime for smoother interpolation
            yield return null;
        }

        underfillBar.fillAmount = targetFillAmount; // Ensure underfillBar matches fillBar
        underfilling = false;
    }

    public void SetFillAmount(int currentHealth, int maxHealth)
    {
        fillBar.fillAmount = (float)((float)currentHealth / (float)maxHealth);
        bartext.text = currentHealth.ToString();
        if (!underfilling)
        {
            StartCoroutine(DelayUnderFill());
        }

    }
}
