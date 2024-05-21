using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEvents : MonoBehaviour
{
    BattleCharacter character;
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


    public void SummonObjectOnTarget(int objectIndex)
    {
        Debug.Log(objectIndex);
        Command c = BattleManager.Singleton.GetActor().currentCommand;
        if(c is SkillCommand)
        {

        Attack skill = (c as SkillCommand).GetAttack();
        string ObjectName = skill.GetSpawnObject(objectIndex);
            if (!string.IsNullOrEmpty(ObjectName))
            {

        GameObject prefab = Resources.Load<GameObject>(ObjectName);
        if (prefab != null)
        {
            GameObject instantiatedObject = Instantiate(prefab, c.Target[0].transform.position, Quaternion.identity);
            Spell spellComponent = instantiatedObject.GetComponent<Spell>();
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
