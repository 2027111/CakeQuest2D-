using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEvents : MonoBehaviour
{
    BattleCharacter character;
    [SerializeField] AudioClip walkSound;
    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetCharacter(BattleCharacter _character)
    {
        character = _character;
    }

    public void TriggerHit()
    {
        character.TriggerHit();
    }



    public void StartParryWindow()
    {
        character.StartParryWindow();
    }
    public void PlayWalkingSound()
    {
        character.PlaySFX(walkSound);
    }




    public void SummonObjectOnTarget(int objectIndex)
    {
        Command c = character.currentCommand;
        if(c is SkillCommand)
        {

        Skill skill = (c as SkillCommand).GetAttack();
        string ObjectName = skill.GetSpawnObject(objectIndex);
            if (!string.IsNullOrEmpty(ObjectName))
            {

        GameObject prefab = Resources.Load<GameObject>(ObjectName);
        if (prefab != null)
        {

                    foreach(BattleCharacter bc in c.Target)
                    {
                        GameObject instantiatedObject = Instantiate(prefab, bc.transform);
                        Spell spellComponent = instantiatedObject.GetComponent<Spell>();
                        if(c.Target.IndexOf(bc) == 0)
                        {
                            Stop();
                            if (spellComponent != null)
                            {
                                // Set the command on the Spell component
                                spellComponent.SetCommand(c);
                                spellComponent.OnOver += Resume;
                            }
                            else
                            {
                                Debug.LogError("The instantiated object does not have a Spell component.");
                            }
                        }
                    }
            
        }
        else
        {
            Debug.LogError($"Prefab with objectIndex '{objectIndex}' could not be found in Resources.");
        }

            }
        }
    }


    public void SummonObjectOnSource(int objectIndex)
    {
        
    }
    public void Stop()
    {

        anim.speed = 0;
    }
    public void Resume()
    {

        anim.speed = 1;
    }
}
