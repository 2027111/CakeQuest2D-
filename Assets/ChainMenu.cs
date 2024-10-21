using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ChainMenu : ChoiceMenu
{



    [SerializeField] GameObject UpButton;
    [SerializeField] GameObject DownButton;
    [SerializeField] GameObject LeftButton;
    [SerializeField] GameObject RightButton;
    [SerializeField] TMP_Text Timer_Text;
    float timer;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void SetTimer(float timer)
    {
        this.timer = timer;
    }
    public string GetTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timer+1 / 60);  // Get the total minutes
        int seconds = Mathf.FloorToInt(timer+1 % 60);  // Get the remaining seconds

        // Return formatted string as "MM:SS"
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Update is called once per frame
    void Update()
    {
        Timer_Text.text = GetTimerDisplay();
        timer -= Time.unscaledDeltaTime;

        if(timer <= 0)
        {
            DestroyMenu();
        }
    }
    public void GiveRandomAttacks(Skill leftSkill, Skill rightSkill)
    {
        LeftButton.GetComponent<RecipeChainButton>().SetCommand(new SkillCommand(leftSkill));
        UpButton.GetComponent<RecipeChainButton>().SetCommand(new AttackCommand());
        RightButton.GetComponent<RecipeChainButton>().SetCommand(new SkillCommand(rightSkill));
        foreach (GameObject button in buttons)
        {
            button.GetComponent<RecipeChainButton>().OnSelected.AddListener(DestroyMenu);
        }
    }

    public void DestroyMenu()
    {

        Destroy(gameObject);
        Utils.ResetTimeScale();
        BattleManager.Singleton?.FadeBackground(false);
    }
    public override void Navigate(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                Select(RightButton);
            }
            else if (direction.x < 0)
            {
                Select(LeftButton);
            }
        }
        else
        {
            if (direction.y > 0)
            {
                Select(UpButton);
            }
            else if (direction.y < 0)
            {
                Select(DownButton);
            }
        }
    }



}
