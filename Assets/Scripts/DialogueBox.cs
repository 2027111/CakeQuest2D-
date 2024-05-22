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
    public Dialogue dialogue;
    public bool choice = false;




    public DialogueContent(Dialogue dialogue)
    {
        this.dialogue = dialogue;
        if (dialogue.choices != null)
        {
            this.choice = dialogue.choices.Length > 0;
        }
    }
}


public class LineInfo{
    public string portraitPath;
    public string line;
    public string talkerName;
    public AudioClip audioClip;

    public LineInfo(string line, string talkerName, string portraitPath)
    {
        this.portraitPath = portraitPath;
        this.line = line;
        this.talkerName = talkerName;
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

    public Stack<UnityAction> OnDialogueOverAction = new Stack<UnityAction>();
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
    }


    public void StartDialogue(Dialogue dialogue, GameObject playerObject = null, GameObject originObject = null, GameState state = GameState.Overworld)
    {



        OnDialogueOverAction.Push(dialogue.OnOverEvent.Invoke); // Push the Invoke method of UnityAction
        currentState = state;
        DialogueContent newDialogue = new DialogueContent(dialogue);

        if (dialogue.dialogueLineIds.Length > 0)
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
                SetupLine(newDialogue.dialogue.dialogueLineIds[0]);
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
                        AddInteractEventToPlayer(true);
                    
                }



                currentDialogue = newDialogue;
                dialogueIndex = 0;
                active = true;
            }
        }
    }


    private void SetupLine(LineInfo lineInfo)
    {
        string portraitPath = lineInfo.portraitPath;
        string talkerName = lineInfo.talkerName;




        portraitImage.gameObject.SetActive(!string.IsNullOrEmpty(portraitPath));

        if (!string.IsNullOrEmpty(portraitPath))
        {
            // Load the sprite from Resources folder
            string fullPath = portraitPath; // Assuming the path is relative to the Resources folder
            Sprite portrait = Resources.Load<Sprite>(fullPath);

            if (portrait == null)
            {
                // Log an error if the sprite failed to load
                Debug.LogError("Failed to load sprite at path: " + fullPath);

                // Optionally, list all loaded sprites for debugging
                portraitImage.gameObject.SetActive(false);

            }
            else
            {
                // Assign the loaded sprite to the portrait image
                portraitImage.sprite = portrait;
            }
        }

        // Set the active state of the name text container based on whether the talker name is provided
        nameTextContainer.SetActive(!string.IsNullOrEmpty(talkerName));

        // Set the text of the name text component to the provided talker name
        nameText.text = string.IsNullOrEmpty(talkerName) ? "" : talkerName;
    }


    private void SetupLine(string lineId)
    {// Set the active state of the portrait image based on whether a sprite is provided


        string portraitPath = LanguageData.GetDataById(lineId).GetValueByKey("portraitPath");
        string talkerName = LanguageData.GetDataById(lineId).GetValueByKey("talkername");




        portraitImage.gameObject.SetActive(!string.IsNullOrEmpty(portraitPath));

        if (!string.IsNullOrEmpty(portraitPath))
        {
            // Load the sprite from Resources folder
            string fullPath = portraitPath; // Assuming the path is relative to the Resources folder
            Sprite portrait = Resources.Load<Sprite>(fullPath);

            if (portrait == null)
            {
                // Log an error if the sprite failed to load
                Debug.LogError("Failed to load sprite at path: " + fullPath);

                // Optionally, list all loaded sprites for debugging
                portraitImage.gameObject.SetActive(false);

            }
            else
            {
                // Assign the loaded sprite to the portrait image
                portraitImage.sprite = portrait;
            }
        }

        // Set the active state of the name text container based on whether the talker name is provided
        nameTextContainer.SetActive(!string.IsNullOrEmpty(talkerName));

        // Set the text of the name text component to the provided talker name
        nameText.text = string.IsNullOrEmpty(talkerName) ? "" : talkerName;
    }


    public void DoChoice(int i)
    {

        
        currentDialogue.choice = false;
        ClearChoiceBox();

        choiceBox.SetActive(false);

        OnDialogueOverAction.Push(currentDialogue.dialogue.choices[i].OnOverEvent.Invoke);
        if (currentDialogue.dialogue != null)
        {

            dialogueWaitingLine.Add(new DialogueContent(new Dialogue(currentDialogue.dialogue.choices[i])));
        }
        Interact();
    }

    private void FillChoiceBox(ChoiceDialogue[] choices)
    {

        ClearChoiceBox();
        AddInteractEventToPlayer(false);
        choiceBox.SetActive(true);
        for (int i = 0; i < choices.Length; i++)
        {
            int number = i;
            Button obj = Instantiate(choicePrefab, choiceBox.transform).GetComponent<Button>();

            string line = LanguageData.GetDataById(choices[i].choicesLineIds).GetValueByKey("line");
            obj.GetComponent<TMP_Text>().text = line;
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
            if (dialogueIndex >= currentDialogue.dialogue.dialogueLineIds.Length)
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
        Interact();
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
                InputManager battleCharacterComponent = player.GetComponent<InputManager>();

                bool contains = battleCharacterComponent.AttackContains(Interact);
                if (battleCharacterComponent != null && !contains)
                {
                    battleCharacterComponent.OnSelectPressed += Interact;
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
                player.GetComponent<InputManager>().OnSelectPressed -= Interact;
            }
        }
    }



    private void NextLine()
    {
        AddInteractEventToPlayer(true);
        DialogueText(currentDialogue.dialogue, dialogueIndex);
    }



    private void ResetBox()
    {
       

        
        dialogueIndex = 0;
        dialogueText.text = "";
        active = false;
        currentDialogue = null;
    }




    private void DialogueText(Dialogue dialogue, int index)
    {
        string lineId = dialogue.dialogueLineIds[index];
        string line = LanguageData.GetDataById(dialogue.dialogueLineIds[index]).GetValueByKey("line");
        string portraitPath = LanguageData.GetDataById(lineId).GetValueByKey("portraitPath");
        string talkerName = LanguageData.GetDataById(lineId).GetValueByKey("talkername");

        if(dialogue.source != null)
        {
            NewDialogueStarterObject component = dialogue.source.GetComponent<NewDialogueStarterObject>();
            line = NewDialogueStarterObject.GetFormattedLines(component, line);

        }



        LineInfo lineInfo = new LineInfo(line, talkerName, portraitPath);




        if (setTextCoroutine != null)
        {
            StopCoroutine(setTextCoroutine);
            setTextCoroutine = null;
            dialogueText.text = line;
            dialogueIndex++;
        }
        else
        {
            SetupLine(lineInfo);
            setTextCoroutine = StartCoroutine(GraduallySetText(line));
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


        yield return StartCoroutine(LanguageData.LoadJsonAsync());
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
            while(OnDialogueOverAction.Count > 0)
            {
                UnityAction currentEvent = OnDialogueOverAction.Pop();
                if (currentEvent != null)
                {
                    currentEvent?.Invoke();
                }

            }
            
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
