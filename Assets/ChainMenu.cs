using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainMenu : ChoiceMenu
{



    [SerializeField] GameObject UpButton;
    [SerializeField] GameObject DownButton;
    [SerializeField] GameObject LeftButton;
    [SerializeField] GameObject RightButton;
    // Start is called before the first frame update
    void Start()
    {

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


    // Update is called once per frame
    void Update()
    {

    }
}
