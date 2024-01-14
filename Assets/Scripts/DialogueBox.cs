using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public enum GameState
{
    Overworld,
    BattleScene
}






[Serializable]
public class DialogueContent
{
    public LineInfo[] lines;
    public Dialogue dialogue;
    public bool choice = false;


    public DialogueContent(LineInfo[] lines)
    {
        this.lines = lines;
    }

    public DialogueContent(Dialogue dialogue)
    {
        this.dialogue = dialogue;
        this.lines = dialogue.dialogueLines;
        this.choice = dialogue.EndInChoice;
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

    public UnityEvent OnDialogueOverAction;
    GameState currentState = GameState.Overworld;
    [SerializeField] CanvasGroup group;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] GameObject choiceBox;
    [SerializeField] GameObject choicePrefab;
    [SerializeField] TMP_Text nameText;
    [SerializeField] Image portraitImage;
    [SerializeField] [Range(0.1f, 1)] float apparitionTime = .4f;
    [SerializeField] [Range(0.1f, 1)] float textSpeed = 0.5f;
    bool isShowing;
    bool active;
    DialogueContent currentDialogue;

    private Button LastButton;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ClearChoiceBox();
            FillChoiceBox(3);
        }
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
                        AddInteractEventToPlayer(true);
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
                        AddInteractEventToPlayer(true);
                    }
                }
                

                
                currentDialogue = newDialogue;
                dialogueIndex = 0;
                active = true;
            }
        }
        
    }
    public void StartDialogue(Dialogue dialogue, GameObject playerObject = null, GameObject originObject = null, GameState state = GameState.Overworld)
    {

        OnDialogueOverAction = dialogue.OnOverEvent;
        currentState = state;
        DialogueContent newDialogue = new DialogueContent(dialogue);

        if (newDialogue.dialogue.dialogueLines.Length > 0)
        {
            if (active)
            {
                Debug.Log("Added Dialogue");
                dialogueWaitingLine.Add(newDialogue);
            }
            else
            {
                Debug.Log("Starting Dialogue");
                dialogueText.text = "";
                SetupLine(newDialogue.dialogue.dialogueLines[0]);
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
                        AddInteractEventToPlayer(true);
                        if (originObject)
                        {
                            player.GetComponent<Character>().LookAt(originObject);
                        }
                    }
                }
                else if (currentState == GameState.BattleScene)
                {
                    if (player)
                    {
                        player.GetComponent<StateMachine>().SetNextState(new EntranceState());
                        AddInteractEventToPlayer(true);
                    }
                }



                currentDialogue = newDialogue;
                dialogueIndex = 0;
                active = true;
            }
        }
    }
    public void StartDialogue(LineInfo[] lines, UnityAction callback,GameObject playerObject = null, GameObject originObject = null, GameState state = GameState.Overworld)
    {
        OnDialogueOverAction.AddListener(callback);
        StartDialogue(lines, playerObject, originObject, state);
    }

    public void SetupLine(LineInfo line)
    {
        portraitImage.gameObject.SetActive(line.portrait != null);
        portraitImage.sprite = line.portrait;
        nameTextContainer.SetActive(!string.IsNullOrEmpty(line.talkername));
        nameText.text = string.IsNullOrEmpty(line.talkername) ? "" : line.talkername;

    }
    public void DoChoice(int i)
    {

        
        currentDialogue.choice = false;
        ClearChoiceBox();

        choiceBox.SetActive(false);
        if (currentDialogue.dialogue != null)
        {

            dialogueWaitingLine.Add(new DialogueContent(currentDialogue.dialogue.choices.dialogues[i]));
        }
        Interact();
    }

    private void FillChoiceBox(ChoiceDialogue choices)
    {

        ClearChoiceBox();
        AddInteractEventToPlayer(false);
        choiceBox.SetActive(true);
        for (int i = 0; i < choices.dialogueLines.Length; i++)
        {
            int number = i;
            Button obj = Instantiate(choicePrefab, choiceBox.transform).GetComponent<Button>();
            obj.GetComponent<TMP_Text>().text = choices.dialogueLines[i];
            obj.onClick.AddListener(delegate { DoChoice(number); } );
            obj.interactable = true;
            obj.Select();

            if (LastButton != null)
            {
                Navigation lastNav = LastButton.GetComponent<Button>().navigation;
                lastNav.mode = Navigation.Mode.Explicit;
                lastNav.selectOnDown = obj;

                Navigation customNav = new Navigation();
                customNav.mode = Navigation.Mode.Explicit;
                customNav.selectOnUp = obj;






                LastButton.GetComponent<Button>().navigation = lastNav;
                obj.navigation = customNav;
            }
            LastButton = obj;

        }
        LastButton = null;
    }
    public void FillChoiceBox(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            Button obj = Instantiate(choicePrefab, choiceBox.transform).GetComponent<Button>();
            obj.onClick.AddListener(DebugTest);
            obj.interactable = true;
            obj.Select();

            if (LastButton != null)
            {
                Navigation lastNav = LastButton.GetComponent<Button>().navigation;
                lastNav.mode = Navigation.Mode.Explicit;
                lastNav.selectOnDown = obj;

                Navigation customNav = new Navigation();
                customNav.mode = Navigation.Mode.Explicit;
                customNav.selectOnUp = obj;
               





                LastButton.GetComponent<Button>().navigation = lastNav;
                obj.navigation = customNav;
            }
            LastButton = obj;

        }
        LastButton = null;
    }

    public void DebugTest()
    {
        Debug.Log("Button clicked");
    }
    public void ClearChoiceBox()
    {
        foreach(Transform child in choiceBox.transform) {
            Destroy(child.gameObject);

        }
        choiceBox.SetActive(false);
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

                        if (currentDialogue.dialogue != null)
                        {
                            if (currentDialogue.choice)
                            {

                                FillChoiceBox(currentDialogue.dialogue.choices);
                                choiceBox.SetActive(true);
                                return;
                            }
                        }


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
            AddInteractEventToPlayer(false);
        }

    }



    public void AddInteractEventToPlayer(bool addOrRemove)
    {
        if (addOrRemove)
        {
            if (currentState == GameState.Overworld)
            {
                Character characterComponent = player.GetComponent<Character>();
                bool contains = characterComponent.InteractContains(Interact);
                if (characterComponent != null && !contains)
                {
                    characterComponent.OnInteractEvent += Interact;
                }
            }
            else if (currentState == GameState.BattleScene)
            {
                BattleCharacter battleCharacterComponent = player.GetComponent<BattleCharacter>();

                bool contains = battleCharacterComponent.AttackContains(Interact);
                if (battleCharacterComponent != null && !contains)
                {
                    battleCharacterComponent.OnAttackPressed += Interact;
                }
            }
        }
        else
        {
            if (currentState == GameState.Overworld)
            {
                player.GetComponent<Character>().OnInteractEvent -= Interact;
            }
            else if (currentState == GameState.BattleScene)
            {
                player.GetComponent<BattleCharacter>().OnAttackPressed -= Interact;
            }
        }
    }



    private void NextLine()
    {
        AddInteractEventToPlayer(true);
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
        ClearChoiceBox();
        EndDialogue();
        ResetBox();
        isShowing = false;
        group.alpha = 0;
    }
    IEnumerator ShowDialogueBoxAlpha(bool show)
    {
        ClearChoiceBox();



        if (isShowing != show)
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
            if(currentDialogue != null)
            {
                NextLine();
            }
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
