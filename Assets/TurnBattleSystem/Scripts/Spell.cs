using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] float[] hitTimers = { 1, 2 };
    bool[] hit;

    float timer = 0;

    Command command;
    public delegate void CommandeEventHandler();
    public CommandeEventHandler OnOver;


    private void Start()
    {
        hit = new bool[hitTimers.Length];
    }

    public void SetCommand(Command _command)
    {
        command = _command;
    }

    private void OnDestroy()
    {
        OnOver?.Invoke();
    }

    private void Update()
    {
        for(int i = 0; i < hitTimers.Length; i++)
        {
            if (!hit[i] && timer > hitTimers[i])
            {
                command?.ActivateCommand();
                hit[i] = true;
            }
        }
        timer += Time.deltaTime;
    }


}
