using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueStarterObject : MonoBehaviour
{
    public LineInfo[] dialogueLines;

    public Character player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }
    public virtual void DialogueAction()
    {
        if (!DialogueBox.Singleton.IsActive())
        {
            DialogueBox.Singleton.StartDialogue(dialogueLines, player.gameObject, gameObject);
        }
    }

    public virtual void DialogueOver()
    {

    }

}
