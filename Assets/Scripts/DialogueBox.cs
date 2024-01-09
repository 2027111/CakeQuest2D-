using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;



public enum GameState
{
    Overworld,
    BattleScene
}






[Serializable]
public class DialogueContent
{
    public LineInfo[] lines;



    public DialogueContent(LineInfo[] lines)
    {
        this.lines = lines;
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
    GameState currentState = GameState.Overworld;
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

    GameObject player = null;
    //DialogueStarterObject starterObject;
    [SerializeField] private GameObject nameTextContainer;

    // Start is called before the first frame update
    void Start()
    {
        Singleton = this;
        group.alpha = 0;
        dialogueIndex = 0;
    }



    public void StartDialogue(LineInfo[] lines, GameObject playerObject = null, GameObject originObject = null, GameState state = GameState.Overworld)
    {
        currentState = state;
        DialogueContent newDialogue = new DialogueContent(lines);

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
                    player = GameObject.FindGameObjectWithTag("Player");
                }
                else
                {

                    player = playerObject;
                }
                if (currentState == GameState.Overworld)
                {
                   
                    if (player)
                    {
                        player.GetComponent<Character>().ChangeState(new InteractingBehaviour());
                        player.GetComponent<Character>().OnInteractEvent += Interact;
                        if (originObject)
                        {
                            player.GetComponent<Character>().LookAt(originObject);
                        }
                    }
                }
                else if(currentState == GameState.BattleScene)
                {
                    if (player)
                    {
                        player.GetComponent<StateMachine>().SetNextState(new EntranceState());
                        player.GetComponent<BattleCharacter>().OnAttackPressed += Interact;
                    }
                }
                

                
                currentDialogue = newDialogue;
                dialogueIndex = 0;
                active = true;
            }
        }
        
    }

    public void StartDialogue(LineInfo[] lines, Action callback,GameObject playerObject = null, GameObject originObject = null, GameState state = GameState.Overworld)
    {
        OnDialogueOverAction = callback;
        StartDialogue(lines, playerObject, originObject, state);
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

                    if (dialogueWaitingLine.Count > 0)
                    {
                        StartNextDialogueWaiting();
                    }
                    else
                    {
                        EndDialogue();
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
    public void StartNextDialogueWaiting()
    {
        currentDialogue = dialogueWaitingLine[0];
        dialogueWaitingLine.RemoveAt(0);
        dialogueIndex = 0;
        NextLine();
    }

    public void EndDialogue()
    {
        if (player)
        {
            if (currentState == GameState.Overworld)
            {

                player.GetComponent<Character>().OnInteractEvent -= Interact;

            }
        }
        else if (currentState == GameState.BattleScene)
        {
            player.GetComponent<BattleCharacter>().OnAttackPressed -= Interact;



            player = null;
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

    public void ForceStop()
    {
        EndDialogue();
        ResetBox();
        isShowing = false;
        group.alpha = 0;
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
