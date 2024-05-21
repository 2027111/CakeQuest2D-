using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChoiceMenuButton : MonoBehaviour
{
    public UnityEvent OnSelected;
    public void SelectFailed()
    {
        DoShake(.3f, .2f);
    }


    private IEnumerator DoShake(float duration, float magnitude)
    {
        Vector3 _originalPos = transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, _originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = _originalPos;
    }


}
