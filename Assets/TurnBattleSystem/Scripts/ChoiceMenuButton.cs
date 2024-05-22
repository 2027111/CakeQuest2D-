using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChoiceMenuButton : MonoBehaviour
{
    public UnityEvent OnSelected;


    [SerializeField] Image BackgroundImage;
    public void SelectFailed()
    {
        Debug.Log("Select failed");
        StartCoroutine(DoShake(.3f, .5f));

    }


    private IEnumerator DoShake(float duration, float magnitude)
    {
        Vector3 _originalPos = BackgroundImage.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            BackgroundImage.transform.localPosition = new Vector3(x, y, _originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        BackgroundImage.transform.localPosition = _originalPos;
    }

    public void SetColor(Color newColor)
    {
        BackgroundImage.color = newColor;
    }
}
