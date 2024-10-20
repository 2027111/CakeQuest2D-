using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : BattleObjects
{
    [SerializeField] float[] hitTimers = { 1, 2 };
    bool[] hit;

    float timer = 0;


    private void Start()
    {
        hit = new bool[hitTimers.Length];
    }



    private void Update()
    {
        for (int i = 0; i < hitTimers.Length; i++)
        {
            if (!hit[i] && timer > hitTimers[i])
            {
                Debug.Log("Command Activated by spell : " + name);
                TriggerHit();
                hit[i] = true;
            }
        }
        timer += Time.deltaTime;
    }



}
