using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Element
{
    None,
    Slash,
    Bash,
    Sweet,
    Sour,
    Bitter,
    Salty,
    Support,
}

public enum ElementEffect
{
    Neutral,
    Weak,
    Resistant,
    NonAffected,
    RecipeBoosted,
    RecipeFailed,
    RecipeCompleted,
    Blocked,
}


[System.Serializable]
public class ElementalAttribute
{
    public Element element;
    public bool found = false;

    public ElementalAttribute()
    {
        element = GetRandomElement();
    }

    private Element GetRandomElement()
    {
        List<Element> validElements = new List<Element>();

        foreach (Element element in System.Enum.GetValues(typeof(Element)))
        {
            if (element != Element.None && element != Element.Support)
            {
                validElements.Add(element);
            }
        }

        Element randomElement = validElements[UnityEngine.Random.Range(0, validElements.Count)];
        return randomElement;
    }
}
