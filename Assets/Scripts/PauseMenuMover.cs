using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuMover : MonoBehaviour
{

    int offsetMovement = 75;
    int index = 0;



    public void ChangeMenu(int leftright)//-1 = left, 1 = right;
    {
        int newIndex = index + leftright;
        if (newIndex >= transform.childCount)
        {
            newIndex = 0;
        }else if (newIndex < 0)
        {
            newIndex = transform.childCount - 1;
        }

        index = newIndex;
        transform.position = new Vector2((index * offsetMovement), 0);
        TurnOffMenu();
    }


    public void TurnOffMenu()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == index);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ChangeMenu(1);
        } else if (Input.GetKeyDown(KeyCode.K))
        {
            ChangeMenu(-1);
        }
    }


}
