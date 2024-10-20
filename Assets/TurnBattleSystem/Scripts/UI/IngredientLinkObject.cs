using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientLinkObject : MonoBehaviour
{

    [SerializeField] string logoPath = "Sprites/logo_element_";
    bool found = false;
    [SerializeField] Image ingredientTypeLogo;
    [SerializeField] Image crossLogo;
    [SerializeField] TMP_Text indexTextNumber;
    public void SetIngredientAttribute(ElementalAttribute element)
    {

        Sprite sprite = Resources.Load<Sprite>(logoPath + element.element.ToString().ToLower());
        if (sprite)
        {
            if (element.found)
            {
                found = element.found;
                ingredientTypeLogo.sprite = sprite;
            }
        }
    }

    public void SetIndex(int index)
    {
        indexTextNumber.SetText($"{index + 1}");
    }


    public void Checked(bool check)
    {

        if (check)
        {
            float randomRotation = Random.Range(-1f, 1f) * 15f;

            crossLogo.transform.rotation = Quaternion.Euler(0, 0, randomRotation);
        }
        crossLogo.gameObject.SetActive(check && found);
    }


}
