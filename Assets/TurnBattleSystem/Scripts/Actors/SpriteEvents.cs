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

    private void Update()
    {
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(character.transform.position.y * 100f) * -1;
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
                        GameObject instantiatedObject = Instantiate(prefab, bc.transform.position, Quaternion.identity);
                        BattleObjects battleObject = instantiatedObject.GetComponent<BattleObjects>();

                        if (battleObject != null)
                        {
                            if (c.Target.IndexOf(bc) == 0)
                            {
                                Stop();
                                // Set the command on the Spell component
                                battleObject.OnOver += Resume;
                            }
                            battleObject.SetTarget(bc);
                            battleObject.SetCommand(c);
                        }
                        else
                        {
                            Debug.LogError("The instantiated object does not have a BattleObjects component.");
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
        Command c = character.currentCommand;
        if (c is SkillCommand)
        {

            Skill skill = (c as SkillCommand).GetAttack();
            string ObjectName = skill.GetSpawnObject(objectIndex);
            if (!string.IsNullOrEmpty(ObjectName))
            {

                GameObject prefab = Resources.Load<GameObject>(ObjectName);
                if (prefab != null)
                {

                    foreach (BattleCharacter bc in c.Target)
                    {
                        GameObject instantiatedObject = Instantiate(prefab, transform.position + Vector3.up, Quaternion.identity);
                        BattleObjects battleObject = instantiatedObject.GetComponent<BattleObjects>();
                            if (battleObject != null)
                            {

                            if (c.Target.IndexOf(bc) == 0)
                            {
                                Stop();
                                // Set the command on the Spell component
                                battleObject.OnOver += Resume;
                            }
                            battleObject.SetTarget(bc);
                            battleObject.SetCommand(c);
                        }
                            else
                            {
                                Debug.LogError("The instantiated object does not have a BattleObjects component.");
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
    public void Stop()
    {

        anim.speed = 0;
    }
    public void Resume()
    {

        anim.speed = 1;
    }
}