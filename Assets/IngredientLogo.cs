using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientLogo : MonoBehaviour
{
    public Element element;
    public Image image;


    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetPresence(bool v)
    {
        image.color = new Color(1,1,1,1)*(v?1:.3f);
    }
}
