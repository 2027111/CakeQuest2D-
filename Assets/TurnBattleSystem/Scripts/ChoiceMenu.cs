using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceMenu : MonoBehaviour
{
    [SerializeField] protected List<GameObject> buttons;
    [SerializeField] protected Color defaultColor = Color.white;
    [SerializeField] protected Color selectedColor = Color.blue;
    protected GameObject SelectedButton;
    public int currentButton;

    private void Start()
    {
        DefaultSelect();
    }
    public void ResetMenu()
    {
        foreach (GameObject button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons.Clear();
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
            SelectedButton.GetComponent<ChoiceMenuButton>().OnOver(false);
        }
        SelectedButton = gameObject;
        if (SelectedButton)
        {
            SelectedButton.GetComponent<ChoiceMenuButton>().SetColor(selectedColor);
            SelectedButton.GetComponent<ChoiceMenuButton>().OnOver(true);
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

    public void AddButton(ChoiceMenuButton obj)
    {
        buttons.Add(obj.gameObject);
    }

}
