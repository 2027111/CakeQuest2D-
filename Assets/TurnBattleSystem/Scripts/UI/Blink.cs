using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequireComponent(typeof(SpriteRenderer))]
public class Blink : MonoBehaviour
{
    Color DefaultColor;
    Color otherColor;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] private float transitionTime = .4f;
    private void Start()
    {
        if (!sr)
        {
            sr = GetComponent<SpriteRenderer>();
            if (!sr)
            {
                sr = GetComponentInChildren<SpriteRenderer>();
            }


            if (!sr)
            {
                Destroy(this);
                return;
            }

        }



        StartCoroutine(Blinking());
    }

    public void SetDefaultColor(Color color)
    {
        DefaultColor = color;
        otherColor = DefaultColor;
        otherColor.a = .65f;
    }

    IEnumerator Blinking()
    {
        bool blinking = true;
        while (blinking)
        {
            Color target = sr.color == DefaultColor ? otherColor : DefaultColor;
            Color start = sr.color;

            float t = 0;
            while (t < transitionTime)
            {
                sr.color = Color.Lerp(start, target, t / transitionTime);
                t += Time.unscaledDeltaTime;
                yield return null;
            }
            sr.color = target;
            yield return null;
        }
    }
}
