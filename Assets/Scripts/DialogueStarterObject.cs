using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueStarterObject : MonoBehaviour
{
    public LineInfo[] dialogueLines;
    public bool started = false;
    public Character player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }
    public virtual void DialogueAction()
    {
        if (!started)
        {
            started = true;
            DialogueBox.Singleton.StartDialogue(dialogueLines, DialogueOver, player.gameObject, gameObject);
        }
    }

    public virtual void DialogueOver()
    {
        started = false;
        Debug.Log("Dialogue Over");
        player.ChangeState(new PlayerControlsBehaviour());
    }

}
