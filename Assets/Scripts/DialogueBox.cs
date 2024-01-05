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


    public LineInfo()
    {
        portrait = null;
        talkername = "";
        line = "";
    }
}





public class DialogueContent
{
    public LineInfo[] lines;
    public GameObject player;
    public GameObject originObject;



    public DialogueContent(LineInfo[] lines, GameObject playerObject = null, GameObject originObject = null)
    {
        this.lines = lines;
        this.player = playerObject;
        this.originObject = originObject;
    }
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

    public Action OnDialogueOverAction;

    [SerializeField] CanvasGroup group;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] Image portraitImage;
    [SerializeField] [Range(0.1f, 1)] float apparitionTime = .4f;
    [SerializeField] [Range(0.1f, 1)] float textSpeed = 0.5f;
    bool isShowing;
    bool active;
    DialogueContent currentDialogue;
    List<DialogueContent> dialogueWaitingLine = new List<DialogueContent>();
    int dialogueIndex = 0;
    Coroutine showBoxCoroutine;
    Coroutine setTextCoroutine;

    Character player = null;
    //DialogueStarterObject starterObject;
    [SerializeField] private GameObject nameTextContainer;

    // Start is called before the first frame update
    void Start()
    {
        Singleton = this;
        group.alpha = 0;
        dialogueIndex = 0;
    }



    public void StartDialogue(LineInfo[] lines, GameObject playerObject = null, GameObject originObject = null)
    {
        DialogueContent newDialogue = new DialogueContent(lines, playerObject, originObject);

        if(lines.Length > 0)
        {
            if (active)
            {
                Debug.Log("Added Dialogue");
                dialogueWaitingLine.Add(newDialogue);
            }
            else
            {
                dialogueText.text = "";
                SetupLine(lines[0]);
                StartCoroutine(Singleton.ShowDialogueBoxAlpha(true));
                if (playerObject == null)
                {
                    player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
                }
                else
                {

                    player = playerObject.GetComponent<Character>();
                }

                

                player.ChangeState(new InteractingBehaviour());
                player.OnInteractEvent += Interact;
                if (originObject)
                {
                    player.LookAt(originObject);
                }
                currentDialogue = newDialogue;
                dialogueIndex = 0;
                active = true;
            }
        }
        
    }

    public void StartDialogue(LineInfo[] lines, Action callback, GameObject playerObject = null, GameObject originObject = null)
    {
        OnDialogueOverAction = callback;
        StartDialogue(lines, playerObject, originObject);
    }

    public void SetupLine(LineInfo line)
    {
        portraitImage.gameObject.SetActive(line.portrait != null);
        portraitImage.sprite = line.portrait;
        nameTextContainer.SetActive(!string.IsNullOrEmpty(line.talkername));
        nameText.text = string.IsNullOrEmpty(line.talkername) ? "" : line.talkername;

    }

    public void Interact()
    {
        if(showBoxCoroutine == null)
        {

        if (active)
        {
            if (dialogueIndex >= currentDialogue.lines.Length)
                {

                    if(dialogueWaitingLine.Count > 0)
                    {
                        currentDialogue = dialogueWaitingLine[0];
                        dialogueWaitingLine.RemoveAt(0);
                        dialogueIndex = 0;
                        NextLine();
                    }
                    else
                    {
                        if (player)
                        {
                            player.OnInteractEvent -= Interact;
                            player = null;
                        }
                        Singleton.StartCoroutine(Singleton.ShowDialogueBoxAlpha(false));
                    }





                }
            else
            {
                if (isShowing)
                {
                    NextLine();
                }





            }
        }
        }
    }
    public void StartDialogue(string[] vs, Action callback, GameObject playerObject = null, GameObject originObject = null)
    {
        LineInfo[] lines = new LineInfo[vs.Length];

        for (int i = 0; i < vs.Length; i++)
        {
            lines[i] = new LineInfo();
            lines[i].portrait = null;
            lines[i].talkername = null;
            lines[i].line = vs[i];
        }

        StartDialogue(lines, callback, playerObject, originObject);
    }
    public void StartDialogue(string[] vs, GameObject playerObject = null, GameObject originObject = null)
    {
        LineInfo[] lines = new LineInfo[vs.Length];
        
        for(int i = 0; i < vs.Length; i++)
        {
            lines[i] = new LineInfo();
            lines[i].portrait = null;
            lines[i].talkername = null;
            lines[i].line  = vs[i];
        }

        StartDialogue(lines, playerObject, originObject);
    }

    private void NextLine()
    {
        DialogueText(currentDialogue.lines[dialogueIndex]);
    }

    private void ResetBox()
    {
       

        
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
            SetupLine(line);
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
        yield return new WaitForSeconds(apparitionTime);
        if (isShowing)
        {
            NextLine();
        }
        else
        {

           ResetBox();
           OnDialogueOverAction?.Invoke();
            
        }
        yield return new WaitForSeconds(.02f);
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
