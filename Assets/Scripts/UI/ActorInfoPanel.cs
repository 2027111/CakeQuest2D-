using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorInfoPanel : MonoBehaviour
{
    [SerializeField] HealthBarUI healthBarUI;
    [SerializeField] GameObject ingredientLinkPrefab;
    [SerializeField] Transform ingredientContainer;
    [SerializeField] IngredientWheel ingredientWheel;


    BattleCharacter Actor;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void SetActor(BattleCharacter actor)
    {
        Actor = actor;
        healthBarUI.SetPlayerRef(Actor);
        SetRecipe();


    }

    private void SetRecipe()
    {
        foreach (Transform child in ingredientContainer)
        {
            Destroy(child.gameObject);
        }
        List<ElementalAttribute> elementalAttributes = Actor.recipe;

        foreach (ElementalAttribute ee in elementalAttributes)
        {
            if (ee != null)
            {
                GameObject ingredient = Instantiate(ingredientLinkPrefab, ingredientContainer);
                IngredientLinkObject obj = ingredient.GetComponent<IngredientLinkObject>();
                obj.SetIngredientAttribute(ee);
                obj.SetIndex(elementalAttributes.IndexOf(ee));
                if (Actor.recipeIndex > elementalAttributes.IndexOf(ee))
                {
                    obj.Checked(true);
                }
            }
        }

        ingredientWheel.SetIngredientWheel(Actor.GetReference().IngredientWheel);
    }

    public void Appear(bool on)
    {
        if (on)
        {
            healthBarUI.SetPlayerRef(BattleManager.Singleton?.GetActor());
        }





        gameObject.SetActive(on);
    }




}
