using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ChainMenu : ChoiceMenu
{



    [SerializeField] GameObject UpButton;
    [SerializeField] GameObject DownButton;
    BattleCharacter battleCharacter;
    [SerializeField] GameObject LeftButton;
    [SerializeField] GameObject RightButton;
    [SerializeField] TMP_Text Timer_Text;


    [SerializeField] AudioClip appearAudioClip;
    [SerializeField] AudioClip confirmAudioClip;
    [SerializeField] AudioClip disappearAudioClip;


    float timer;
    private void Start()
    {
        battleCharacter.PlaySFX(appearAudioClip);
    }
    public void Confirm()
    {

        PlayTakeOverClip();

        battleCharacter.PlaySFX(confirmAudioClip);
    }


    public void SetTimer(float timer)
    {
        this.timer = timer;
    }
    public string GetTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timer / 60);  // Get the total minutes
        int seconds = Mathf.FloorToInt(timer % 60);  // Get the remaining seconds
        int milliseconds = Mathf.FloorToInt((timer * 100) % 100);  // Get the milliseconds

        // Return formatted string as "MM:SS:MS"
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
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

    public async void PlayTakeOverClip()
    {
        AudioClip TakeOverClip = await Utils.GetVoiceLine($"Battle_{battleCharacter.GetData().characterName}_TakeOver");
       // battleCharacter.PlayVoiceLine(TakeOverClip);

    }
    public void GiveBattleCharacter(BattleCharacter performer)
    {
        battleCharacter = performer;

        LeftButton.GetComponent<RecipeChainButton>().SetCommand(battleCharacter.CreateCommand());
        UpButton.GetComponent<RecipeChainButton>().SetCommand(battleCharacter.CreateCommand());
        RightButton.GetComponent<RecipeChainButton>().SetCommand(battleCharacter.CreateCommand());

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

    public void GiveRandomCommand(Command leftSkill, Command rightSkill)
    {
        LeftButton.GetComponent<RecipeChainButton>().SetCommand(leftSkill);
        //UpButton.GetComponent<RecipeChainButton>().SetCommand(new AttackCommand());
        RightButton.GetComponent<RecipeChainButton>().SetCommand(rightSkill);

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

        PlayTakeOverClip();
        battleCharacter.PlaySFX(disappearAudioClip);
        BattleManager.Singleton?.GetActor().GiveNextCommand(SelectedButton.GetComponent<RecipeChainButton>().GetCommand());
        DestroyMenu();

    }



}
