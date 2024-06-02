using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{

    private static UICanvas _singleton;

    public static UICanvas Singleton
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
                Debug.Log($"{nameof(UICanvas)} instance already exists. Destroying duplicate!");
                Destroy(value.gameObject);
            }
        }
    }



    [SerializeField] UIBorder border;
    [SerializeField] QuestList questList;
    [SerializeField] PartyList partyList;

    private void Start()
    {
        Singleton = this;
        DontDestroyOnLoad(Singleton);
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
