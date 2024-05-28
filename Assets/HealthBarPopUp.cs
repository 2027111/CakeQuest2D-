using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarPopUp : MonoBehaviour
{
    [SerializeField] GameObject healthBar;
    [SerializeField] float timeTillDest = 1.1f;
    GameObject currentBar;

    public void OnHealthChange(int currentAmount, int maxAmount)
    {
        if(currentBar == null)
        {
            currentBar = Instantiate(healthBar, transform);
        }
        currentBar.GetComponentInChildren<HealthBar>().SetFillAmount(currentAmount, maxAmount);
        StopCoroutine(DestroyPopup(timeTillDest));
        StartCoroutine(DestroyPopup(timeTillDest));
    }



    public IEnumerator DestroyPopup(float time)
    {
        float t = 0;
        while(t < time)
        {
            t += Time.deltaTime;
            yield return null;
        }

        if (currentBar)
        {
            Destroy(currentBar);
        }
        yield return null;
    }


}
