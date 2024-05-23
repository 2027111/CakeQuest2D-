using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceMenu : MonoBehaviour
{
    [SerializeField] protected List<GameObject> buttons;
    [SerializeField] protected Color defaultColor;
    [SerializeField] protected Color selectedColor;
    protected GameObject SelectedButton;
    public int currentButton;

    private void Start()
    {
        DefaultSelect();
    }
    public void ResetMenu()
    {
        foreach (Transform transform in transform)
        {
            Destroy(transform.gameObject);
        }
    }
    public void DefaultSelect()
    {

        if (buttons.Count > 0)
        {
            Select(buttons[currentButton]);
        }
    }

    protected void Select(GameObject gameObject)
    {
        if (SelectedButton)
        {
            SelectedButton.GetComponent<ChoiceMenuButton>().SetColor(defaultColor);
        }
        SelectedButton = gameObject;
        if (SelectedButton)
        {
            SelectedButton.GetComponent<ChoiceMenuButton>().SetColor(selectedColor);
        }
    }

    public void TriggerSelected()
    {
        if (!SelectedButton)
        {
            DefaultSelect();
        }
        SelectedButton.GetComponent<ChoiceMenuButton>()?.OnSelected?.Invoke();
    }

    public string GetSelectedOption()
    {
        return SelectedButton.GetComponent<ButtonObject>().GetText();
    }

    public virtual void NextButton()
    {
        currentButton++;
        if (currentButton >= buttons.Count)
        {
            currentButton = 0;
        }
        Select(buttons[currentButton]);

    }


    public virtual void PreviousButton()
    {
        currentButton--;
        if (currentButton < 0)
        {
            currentButton = buttons.Count - 1;
        }
        Select(buttons[currentButton]);
    }



}
