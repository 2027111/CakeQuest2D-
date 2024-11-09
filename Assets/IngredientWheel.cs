using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientWheel : MonoBehaviour
{
    [SerializeField] IngredientLogo[] IngredientLogos;

    public void SetIngredientWheel(List<Element> possibilities)
    {
        foreach(IngredientLogo logo in IngredientLogos)
        {
            logo.SetPresence(possibilities.Contains(logo.element));
        }
    }




}
