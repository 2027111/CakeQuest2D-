using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBarPopUp : MonoBehaviour
{
    [SerializeField] GameObject healthBar;
    [SerializeField] float timeTillDest = 1.1f;
    GameObject currentBar;

    public void OnHealthChange(int currentAmount, int maxAmount)
    {
        if (currentBar == null)
        {
            currentBar = Instantiate(healthBar, transform);
            currentBar.GetComponentInChildren<HealthBar>().OnFillAmountReached.AddListener(DestroyPopup);
        }
        currentBar.GetComponentInChildren<HealthBar>().SetFillAmount(currentAmount, maxAmount);



    }

    public void DestroyPopup()
    {
        StartCoroutine(DestroyIn());
    }

    public IEnumerator DestroyIn()
    {
        yield return new WaitForSeconds(timeTillDest);
        Destroy(currentBar);

    }


}
