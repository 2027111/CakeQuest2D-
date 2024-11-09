using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public bool[] menus = new bool[5]; //Atttack, Skills, Items, Swap, Observe
    private void Awake()
    {
        ResetMenus();
    }
    public void DisableMenus(int[] vs)
    {
        foreach(int i in vs)
        {
            menus[i] = false;
        }
    }

    public void ResetMenus()
    {
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i] = true;
        }
    }


    public bool GetDisability(int index)
    {
        return !menus[index];
    }
}
