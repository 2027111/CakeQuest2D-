using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextObject : MonoBehaviour
{

    [SerializeField] TMP_Text indicationText;
    float limit = .4f;



    public void Setup(string text, Color color)
    {
        



        indicationText.text = text;
        indicationText.color = color;


    }
    private void Update()
    {
        transform.position += Vector3.up * Time.deltaTime;
        Color color = indicationText.color;
        limit -= Time.deltaTime;
        color.a = limit;
        indicationText.color = color;


        if(limit <= 0)
        {
            Destroy(gameObject);
        }

    }
}