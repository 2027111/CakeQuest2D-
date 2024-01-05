using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
[Serializable] public class LineInfo
{
    public Sprite portrait;
    public string talkername;
    public string line;
}
public class DialogueBox : MonoBehaviour
{
    private static DialogueBox _singleton;

    public static DialogueBox Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {

                _singleton = value;
            }
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(DialogueBox)} instance already exists. Destroying duplicate!");
                Destroy(value.gameObject);
            }
        }
    }



    [SerializeField] CanvasGroup group;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] Image portraitImage;
    [SerializeField] float apparitionTime = .4f;
    [SerializeField] [Range(0.1f, 1)] float textSpeed = 0.5f;
    bool isShowing;
    bool active;
    LineInfo[] currentDialogue;
    int dialogueIndex = 0;
    Coroutine showBoxCoroutine;
    Coroutine setTextCoroutine;

    Character player = null;
    DialogueStarterObject starterObject;
    // Start is called before the first frame update
    void Start()
    {
        Singleton = this;
        group.alpha = 0;
        dialogueIndex = 0;
    }



    public void StartDialogue(LineInfo[] lines, GameObject playerObject = null, GameObject originObject = null)
    {

        dialogueText.text = "";
        StartCoroutine(Singleton.ShowDialogueBoxAlpha(true));
        if (playerObject == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        }
        else
        {

            player = playerObject.GetComponent<Character>();
        }

        if (originObject)
        {
            starterObject = originObject.GetComponent<DialogueStarterObject>();
        }

        player.ChangeState(new InteractingBehaviour());
        player.OnInteractEvent += Interact;
        if (originObject)
        {
            player.LookAt(originObject);
        }
        currentDialogue = lines;
        dialogueIndex = 0;
        active = true;
    }

    public void Interact()
    {
        if(showBoxCoroutine == null)
        {

        if (active)
        {
            if (dialogueIndex >= currentDialogue.Length)
            {
                ResetBox();
                Singleton.StartCoroutine(Singleton.ShowDialogueBoxAlpha(false));
            }
            else
            {
                if (isShowing)
                {
                    NextLine();
                }





            }
        }
        else
        {
            StartDialogue(new string[] { "I like pedophiles and kids", "stop using such terminology it is unnacceptable" });
        }
        }
    }

    private void StartDialogue(string[] vs)
    {
        LineInfo[] lines = new LineInfo[vs.Length];
        for(int i = 0; i < vs.Length; i++)
        {
            lines[i].portrait = null;
            lines[i].talkername = null;
            lines[i].line  = vs[i];
        }

        StartDialogue(lines);
    }

    private void NextLine()
    {
        DialogueText(currentDialogue[dialogueIndex]);
    }

    private void ResetBox()
    {
        if (player)
        {
            player.ChangeState(new PlayerControlsBehaviour());
            player.OnInteractEvent -= Interact;
            player = null;
        }

        if (starterObject)
        {
            starterObject.DialogueOver();
        }
        dialogueIndex = 0;
        dialogueText.text = "";
        active = false;
        currentDialogue = null;
    }


    public void DialogueText(LineInfo line)
    {
        if (setTextCoroutine != null)
        {
            StopCoroutine(setTextCoroutine);
            setTextCoroutine = null;
            dialogueText.text = line.line;
            dialogueIndex++;
        }
        else
        {
            portraitImage.sprite = line.portrait;
            nameText.text = string.IsNullOrEmpty(line.talkername)?"":line.talkername;
            setTextCoroutine = StartCoroutine(GraduallySetText(line.line));
        }

    }
    public bool IsActive()
    {
        return active;
    }
    IEnumerator ShowDialogueBoxAlpha(bool show)
    {
        if(isShowing != show)
        {

        float target = show ? 1 : 0;
        float start = group.alpha;
        float duration = 0;
        while(duration < apparitionTime)
        {
            group.alpha = Mathf.Lerp(start, target, duration / apparitionTime);
            duration += Time.deltaTime;
            yield return null;
        }
            group.alpha = target;
            isShowing = show;
        }
        if (isShowing)
        {
            NextLine();
        }
        yield return null;
    }

    IEnumerator GraduallySetText(string text)
    {
        if (isShowing)
        {
            dialogueText.text = "";


            for(int i = 0; i < text.Length; i++)
            {
                dialogueText.text += text[i];
                yield return new WaitForSeconds((1.1f - textSpeed) / 10);
            }
            dialogueIndex++;
            setTextCoroutine = null;
        }
    }



}
