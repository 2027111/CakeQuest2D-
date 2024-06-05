using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBorder : MonoBehaviour
{

    public float disappearScale = 1.2f; // Set the desired scale when disappearing
    public float timeToScale = .30f; // Duration to scale

    public void Appear(bool appear)
    {
        StopCoroutine("ChangeUIScale");
        StartCoroutine(ChangeUIScale(appear));
    }

    public IEnumerator ChangeUIScale(bool appear)
    {
        float targetScale = appear ? 1.0f : disappearScale;
        Vector3 startScale = transform.localScale;
        Vector3 targetScaleV = new Vector3(targetScale, targetScale, targetScale);
        float t = 0.0f;

        while (t < timeToScale)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScaleV, t / timeToScale);
            yield return null;
        }

        transform.localScale = targetScaleV;
    }


}
