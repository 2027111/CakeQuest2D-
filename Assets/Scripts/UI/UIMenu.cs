using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenu : MonoBehaviour
{
    [SerializeField] CanvasGroup menu;

    private void Awake()
    {
        menu = GetComponent<CanvasGroup>();
    }

    private void OnValidate()
    {
        menu = GetComponent<CanvasGroup>();

    }

    public void OpenMenu()
    {

        menu.alpha = 1;
        menu.interactable = true;
        menu.blocksRaycasts = true;
    }

    public void CloseMenu()
    {

        menu.alpha = 0;
        menu.interactable = false;
        menu.blocksRaycasts = false;
    }
}
