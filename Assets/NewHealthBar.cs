using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class NewHealthBar : MonoBehaviour
{

    [SerializeField] Slider fillBar;
    [SerializeField] Image underfillBar;
    [SerializeField] TextMeshProUGUI bartext;
    [SerializeField] TextMeshProUGUI barNameText;
    [SerializeField] float delayTime = 1;
    [SerializeField] float catchUpTime = 1;
    bool underfilling = false;
    public UnityEvent OnFillAmountReached;



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
        float targetFillAmount = fillBar.value;

        while (underfillBar.fillAmount > fillBar.value)
        {
            float newFill = Mathf.Lerp(startFill, targetFillAmount, time);
            underfillBar.fillAmount = newFill;
            time += Time.deltaTime * catchUpTime; // Use Time.deltaTime for smoother interpolation
            yield return null;
        }

        underfillBar.fillAmount = targetFillAmount; // Ensure underfillBar matches fillBar
        underfilling = false;
        OnFillAmountReached?.Invoke();
    }

    public void SetFillAmount(int currentHealth, int maxHealth)
    {
        if (!underfilling && underfillBar != null)
        {
            underfillBar.fillAmount = fillBar.value;
            StartCoroutine(DelayUnderFill());
        }
        fillBar.value = (float)((float)currentHealth / (float)maxHealth);
        bartext?.SetText($"{currentHealth}/{maxHealth}");

    }
}
