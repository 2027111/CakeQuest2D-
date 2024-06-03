using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{

    private static UICanvas _singleton;
    public static UICanvas Singleton
    {
        get
        {
            if (_singleton == null)
            {
                // Load the MusicPlayer prefab from Resources
                GameObject canvasPrefab = Resources.Load<GameObject>("UICanvas");
                if (canvasPrefab != null)
                {
                    GameObject canvasInstance = Instantiate(canvasPrefab);
                    Singleton = canvasInstance.GetComponent<UICanvas>();
                    Debug.Log("UICanvas Instantiated");
                }
                else
                {
                    Debug.LogError("UICanvas prefab not found in Resources.");
                }
            }
            return _singleton;
        }
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(UICanvas)} instance already exists. Destroying duplicate!");
                Destroy(value.gameObject);
            }
        }
    }



    void Awake()
    {
        if (_singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(Singleton.gameObject);
        }
        else if (_singleton != this)
        {
            Destroy(this.gameObject);
        }
    }



    [SerializeField] UIBorder border;
    [SerializeField] QuestList questList;
    [SerializeField] PartyList partyList;
    [SerializeField] DialogueBox dialogueBox;


    public static void StartDialogue(Dialogue dialogue, GameObject playerObject = null, GameObject originObject = null, GameState state = GameState.Overworld)
    {
        Singleton?.dialogueBox.StartDialogue(dialogue, playerObject, originObject, state);
    }
    public static void ForceStopDialogue()
    {
        Singleton?.dialogueBox.ForceStop();
    }

    public static bool DialogueBoxIsActive()
    {
        return Singleton.dialogueBox.IsActive();
    }

    public static void CancelCurrentDialogue()
    {

        Singleton?.dialogueBox.CancelDialogue();
    }


    public static void TurnBordersOn(bool on)
    {
        Singleton?.border.Appear(on);
        Singleton?.questList.Appear(on);
        Singleton?.partyList.Appear(on);
    }

    public static void UpdateQuestList()
    {
        Singleton?.questList?.ResetList();
    }

    public static void UpdatePartyList()
    {
        Singleton?.partyList?.UpdateList();
    }
}
