using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;
using System.Text.RegularExpressions;

public enum GameState
{
    Overworld,
    BattleScene
}






[Serializable]
public class DialogueContent
{
    public Dialogue dialogue;
    //public bool choice = false;

    public DialogueContent(Dialogue dialogue)
    {
        this.dialogue = dialogue;

    }
}


public class LineInfo{
    public string lineId;
    public string line;
    public string talkerName;
    public string portraitPath;
    public bool voiced = false;
    public AudioClip audioClip;

    public LineInfo(string lineId)
    {
        this.lineId = lineId;
        this.line = LanguageData.GetDataById(lineId).GetValueByKey("line");
        this.talkerName = LanguageData.GetDataById(lineId).GetValueByKey("talkerName");
        this.portraitPath = LanguageData.GetDataById(lineId).GetValueByKey("portraitPath");
        if (bool.TryParse(LanguageData.GetDataById(lineId).GetValueByKey("voiced"), out bool result))
        {
            voiced = result;
        }
    }



    public LineInfo(string lineId, string line, string talkerName, string portraitPath)
    {
        this.lineId = lineId;
        this.line = line;
        this.talkerName = talkerName;
        this.portraitPath = portraitPath;
        if(bool.TryParse(LanguageData.GetDataById(lineId).GetValueByKey("voiced"), out bool result)){
            voiced = result;
        }
    }
}



public class DialogueBox : MonoBehaviour
{

    public Queue<UnityAction> OnDialogueOverAction = new Queue<UnityAction>();
    GameState currentState = GameState.Overworld;
    [Header("References")]
    [SerializeField] GameObject choiceBox;
    [SerializeField] CanvasGroup group;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] GameObject portraitContainer;
    [SerializeField] Image portraitImage;
    [SerializeField] AudioSource voiceClipSource;
    [SerializeField] private GameObject nameTextContainer;

    [Header("Prefabs")]
    [SerializeField] GameObject choicePrefab;



    [Header("Attributes")]
    [SerializeField] [Range(0.1f, 1)] float apparitionTime = .4f;
    [SerializeField] [Range(5, 60)] int textCharPerSecond = 30;



    [Header("InnerAttributes")]
    bool isShowing;
    bool active;
    DialogueContent currentDialogue;
    List<DialogueContent> dialogueWaitingLine = new List<DialogueContent>();
    int dialogueIndex = 0;
    bool dontGoNext = false;
    Coroutine showBoxCoroutine;
    Coroutine setTextCoroutine;
    GameObject player = null;

    GameObject Player 
    {
        get
        {
            if(player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            return player;
        }
        set
        {
            player = value;
        }

    }

    public void DontGoNext()
    {
        dontGoNext = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        group.alpha = 0;
        dialogueIndex = 0;
    }

    public IEnumerator WaitForResume()
    {
        yield return MakeBoxAppear(false);
        AddInteractEventToPlayer(false);
    }



    public IEnumerator Resume()
    {

        if (isShowing)
        {
            dontGoNext = false;
            yield return MakeBoxAppear(true);
            AddInteractEventToPlayer(true);
            Interact();
        }
    }
    private void Update()
    {
    }

    public void CancelDialogue()
    {
        Debug.Log("Cancel Dialogue");
        ForceStop();
        StartCoroutine(ShowDialogueBoxAlpha(false));

    }
    public void StartDialogueDelayed(Dialogue dialogue, GameObject playerObject, GameObject originObject, GameState state)
    {
        if (dialogue.OnOverEvent != null)
        {
            OnDialogueOverAction.Enqueue(dialogue.OnOverEvent.Invoke); // Push the Invoke method of UnityAction
        }
        currentState = state;
        DialogueContent newDialogue = new DialogueContent(dialogue);


        if (dialogue.dialogueLineIds != null)
        {
            if (dialogue.dialogueLineIds.Length > 0)
            {

                Debug.Log("Added waiting dialogue");
                dialogueWaitingLine.Add(newDialogue);

            }
        }

     }

    public void StartDialogue(Dialogue dialogue, GameObject playerObject = null, GameObject originObject = null, GameState state = GameState.Overworld)
    {


        if(dialogue.OnOverEvent != null)
        {
            OnDialogueOverAction.Enqueue(dialogue.OnOverEvent.Invoke); // Push the Invoke method of UnityAction
        }
        currentState = state;
        DialogueContent newDialogue = new DialogueContent(dialogue);
        
        if (dialogue.dialogueLineIds != null)
        {

            if (dialogue.dialogueLineIds.Length > 0)
            {

                if (active)
                {
                    //Debug.Log("Added Dialogue");
                    dialogueWaitingLine.Add(newDialogue);
                }
                else
                {
                    //Debug.Log("Starting Dialogue");
                    dialogueText.text = "";
                    string lineId = newDialogue.dialogue.dialogueLineIds[0];
                    LineInfo lineInfo = new LineInfo(lineId);
                    SetupLine(lineInfo, false);
                    StartCoroutine(ShowDialogueBoxAlpha(true));

                    if (playerObject == null)
                    {
                        Player = GameObject.FindGameObjectWithTag("Player");
                    }
                    else
                    {

                        Player = playerObject;
                    }
                    if (currentState == GameState.Overworld)
                    {

                        if (Player)
                        {
                            Player.GetComponent<Character>().ChangeState(new InteractingBehaviour());
                            AddInteractEventToPlayer(true);
                            if (originObject)
                            {
                                Player.GetComponent<Character>().LookAt(originObject);
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
    }


    private void SetupLine(LineInfo lineInfo, bool playVoiceLine = true)
    {
        string portraitPath = lineInfo.portraitPath;
        string talkerName = lineInfo.talkerName;



        portraitContainer.gameObject.SetActive(!string.IsNullOrEmpty(portraitPath));

        if (!string.IsNullOrEmpty(portraitPath))
        {
            // Load the sprite from Resources folder
            string fullPath = portraitPath; // Assuming the path is relative to the Resources folder
            Sprite portrait = Resources.Load<Sprite>(fullPath);

            if (portrait == null)
            {
                // Log an error if the sprite failed to load
                Debug.LogWarning("Failed to load sprite at path: " + fullPath);

                // Optionally, list all loaded sprites for debugging
                portraitContainer.gameObject.SetActive(false);

            }
            else
            {
                // Assign the loaded sprite to the portrait image
                portraitImage.sprite = portrait;
            }
        }




        if (lineInfo.voiced && playVoiceLine)
        {
            // Load the audio from Resources folder

            PlayLineVoiceClip(lineInfo.lineId);



        }



        // Set the active state of the name text container based on whether the talker name is provided
        nameTextContainer.SetActive(!string.IsNullOrEmpty(talkerName));

        // Set the text of the name text component to the provided talker name
        nameText.text = string.IsNullOrEmpty(talkerName) ? "" : talkerName;
    }

    public void PlayLineVoiceClip(string lineId)
    {
        AudioClip voiceLine = Utils.GetVoiceLine(lineId);

        if (voiceLine != null)
        {
            // Assign the loaded audio to the audio source
            if(voiceClipSource.clip != voiceLine)
            {


                voiceClipSource?.Stop();
                voiceClipSource.clip = voiceLine;
                voiceClipSource.Play();
            }
        }

    }
    public void DoChoice(ChoiceDialogue choice)
    {

        //currentDialogue.choice = false;
        ClearChoiceBox();
        choiceBox.SetActive(false);

        OnDialogueOverAction.Enqueue(choice.OnOverEvent.Invoke);
        if (currentDialogue.dialogue != null)
        {

            dialogueWaitingLine.Insert(0, new DialogueContent(new Dialogue(choice)));
        }

        AddNavigateEventToPlayer(false);
        StartNextDialogueWaiting();
    }



    private void FillChoiceBox(ChoiceDialogue[] choices)
    {

        ClearChoiceBox();
        AddInteractEventToPlayer(false);
        AddNavigateEventToPlayer(true);
        choiceBox.SetActive(true);
        for (int i = 0; i < choices.Length; i++)
        {
            int number = i;
            ChoiceDialogue choice = choices[number];
            ChoiceMenuButton obj = Instantiate(choicePrefab, choiceBox.transform).GetComponent<ChoiceMenuButton>();
            LineInfo choiceLine = new LineInfo(choices[i].choicesLineIds);
            obj.GetComponent<TMP_Text>().text = choiceLine.line;
            obj.OnSelected.AddListener(delegate { DoChoice(choice); });
            obj.SetMenu(choiceBox.GetComponent<ChoiceMenu>());
            choiceBox.GetComponent<ChoiceMenu>().AddButton(obj);
            // obj.Select();



        }
        choiceBox.GetComponent<ChoiceMenu>().DefaultSelect();
    }
  
    public void DebugTest()
    {
    }

    public void FlipPortrait()
    {
        float side = Mathf.Sign(portraitContainer.transform.localScale.x);
        float nextSide = side * -1f;
        float xPos = side * 28;




    }
    public void ClearChoiceBox()
    {
        choiceBox.GetComponent<ChoiceMenu>().ResetMenu();
        choiceBox.SetActive(false);
    }

    public void Interact()
    {
        if (showBoxCoroutine == null)
        {

            if (active)
            {
                int dialogueLength = 0;
                if(currentDialogue.dialogue.dialogueLineIds != null)
                {
                    dialogueLength = currentDialogue.dialogue.dialogueLineIds.Length;
                }
                if (dialogueIndex >= dialogueLength)
                {

                    if (currentDialogue != null)
                    {
                        if(currentDialogue.dialogue.OnInstantOverEvent != null)
                        {
                            currentDialogue.dialogue.OnInstantOverEvent?.Invoke();

                            if (currentDialogue != null)
                            {
                                currentDialogue.dialogue.OnInstantOverEvent = null;
                            }
                        }
                    }

                    if (dontGoNext)
                    {
                        dontGoNext = false;
                        return;
                    }else if (currentDialogue != null)
                    {


                    

                        ChoiceDialogue[] choices = currentDialogue.dialogue.GetUsableChoices();



                        if (choices != null)
                        {
                            if (choices.Length == 1)
                            {
                                if (choices[0].ConditionRespected())
                                {
                                    dialogueWaitingLine.Insert(0,new DialogueContent(new Dialogue(choices[0])));
                                    StartNextDialogueWaiting();
                                    return;
                                }
                            }
                            else
                            {

                                FillChoiceBox(choices);
                                choiceBox.SetActive(true);
                                return;
                            }
                        }
                        else if (dialogueWaitingLine.Count > 0)
                        {
                            StartNextDialogueWaiting();

                            return;



                        }



                        voiceClipSource?.Stop();
                        EndDialogue();
                        StartCoroutine(ShowDialogueBoxAlpha(false));

                   


                }




                }
                else
                {
                    if (isShowing)
                    {

                        if (CurrentLine() != null)
                        {

                            NextLine();
                        }
                        else
                        {
                            EndDialogue();
                            StartCoroutine(ShowDialogueBoxAlpha(false));

                        }
                    }






                }
            }
        }
    }

    private JsonData CurrentLine()
    {
        return LanguageData.GetDataById(currentDialogue.dialogue.dialogueLineIds[dialogueIndex]);

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
        if (Player)
        {
            AddInteractEventToPlayer(false);
        }

    }



    public void AddInteractEventToPlayer(bool addOrRemove)
    {
        if (addOrRemove)
        {
            Controller battleCharacterComponent = Player.GetComponent<Controller>();

            bool contains = battleCharacterComponent.AttackContains(Interact);
            if (battleCharacterComponent != null && !contains)
            {
                battleCharacterComponent.OnSelectPressed += Interact;
            }

        }
        else
        {
            Controller battleCharacterComponent = Player.GetComponent<Controller>();
            bool contains = battleCharacterComponent.AttackContains(Interact);
            if (battleCharacterComponent != null && contains)
            {
                battleCharacterComponent.OnSelectPressed -= Interact;
            }

        }
    }
    public void AddNavigateEventToPlayer(bool addOrRemove)
    {
        Controller battleCharacterComponent = Player.GetComponent<Controller>();
        if (addOrRemove)
        {
            battleCharacterComponent.OnMovementPressed += NavigateMenu;
            battleCharacterComponent.OnSelectPressed += choiceBox.GetComponent<ChoiceMenu>().TriggerSelected;

        }
        else
        {
            battleCharacterComponent.OnMovementPressed -= NavigateMenu;
            battleCharacterComponent.OnSelectPressed -= choiceBox.GetComponent<ChoiceMenu>().TriggerSelected;

        }
    }

    public void NavigateMenu(Vector2 direction)
    {
        ChoiceMenu menu = choiceBox.GetComponent<ChoiceMenu>();
        if (menu != null)
        {
            if(direction.y > 0)
            {
                menu.PreviousButton();
            }
            if (direction.y < 0)
            {
                menu.NextButton();
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
        if(index < dialogue.dialogueLineIds.Length)
        {

        string lineId = dialogue.dialogueLineIds[index];
        LineInfo lineInfo = new LineInfo(lineId);

            if (dialogue.source != null)
            {
                lineInfo.line = NewDialogueStarterObject.GetFormattedLines(dialogue.source, lineInfo.line);

            }






        Debug.Log("This is called twice");
        if (setTextCoroutine != null)
        {
            StopCoroutine(setTextCoroutine);
            setTextCoroutine = null;
            dialogueText.text = lineInfo.line;
            dialogueIndex++;
        }
        else
        {
            voiceClipSource.Stop();
            SetupLine(lineInfo);
            setTextCoroutine = StartCoroutine(GraduallySetText(lineInfo.line));
            }
        }
    }


    public bool IsActive()
    {
        return active;
    }

    public void ForceStop()
    {
        EndDialogue();
        ClearChoiceBox();
        ResetBox();
        isShowing = false;
        group.alpha = 0;
    }

    IEnumerator MakeBoxAppear(bool show)
    {

            float target = show ? 1 : 0;
            float start = group.alpha;
            float duration = 0;
            while (duration < apparitionTime)
            {
                group.alpha = Mathf.Lerp(start, target, duration / apparitionTime);
                duration += Time.deltaTime;
                yield return null;
            }
            group.alpha = target;


        yield return new WaitForSeconds(apparitionTime);
    }
    IEnumerator ShowDialogueBoxAlpha(bool show)
    {
        ClearChoiceBox();


        if(isShowing != show)
        {
            isShowing = show;
            if (!isShowing)
            {

                ResetBox();
                while (OnDialogueOverAction.Count > 0)
                {
                    UnityAction currentEvent = OnDialogueOverAction.Dequeue();
                    currentEvent?.Invoke();
                    yield return null;

                }

            }
            yield return MakeBoxAppear(show);

            if (isShowing)
            {
                if (currentDialogue != null)
                {
                    yield return StartCoroutine(LanguageData.LoadJsonAsync());
                    NextLine();
                }
            }
            yield return new WaitForSeconds(.02f);
        }
    }

    IEnumerator GraduallySetText(string text)
    {
        if (isShowing)
        {
            dialogueText.text = "";
            string line = "";

            // Define the pattern to match <anything> or any word without tags
            string pattern = @"<[^>]+>|[^<\s]+";

            // Match all words and tags
            MatchCollection matches = Regex.Matches(text, pattern);

            foreach (Match match in matches)
            {
                string wordOrTag = match.Value;

                // If the text contains a tag, append it instantly
                if (Regex.IsMatch(wordOrTag, @"<[^>]+>"))
                {
                    line += wordOrTag;
                }
                else
                {
                    // Gradually append each character of the word
                    for (int j = 0; j < wordOrTag.Length; j++)
                    {
                        line += wordOrTag[j];
                        dialogueText.SetText(line);
                        yield return new WaitForSeconds(1f / textCharPerSecond);
                    }

                    // Add a space after the word if the next match is not a closing tag
                    if (match.NextMatch().Success && !match.NextMatch().Value.StartsWith("</"))
                    {
                        line += " ";
                    }
                }

                dialogueText.SetText(line);
            }
            dialogueText.SetText(text);
            dialogueIndex++;
            setTextCoroutine = null;
        }
    }

}




