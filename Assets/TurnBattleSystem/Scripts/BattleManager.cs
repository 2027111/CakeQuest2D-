using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    private static BattleManager _singleton;
    public static BattleManager Singleton
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
                Debug.Log($"{nameof(BattleManager)} instance already exists. Destroying duplicate!");
                Destroy(value.gameObject);
            }
        }
    }



    public bool FastCombats;

    [SerializeField] TMP_Text DebugBattleStateText;
    [SerializeField] TMP_Text battleControlText;
    [SerializeField] TMP_Text battleIndicationText;



    public GameObject BattlePrefab;
    [SerializeField] GameObject CursorPrefab;
    [SerializeField] GameObject CardUIPrefab;
    [SerializeField] Transform CardUIContainer;
    [SerializeField] Transform PlayerSpawnPoint;
    [SerializeField] Transform EnemySpawnPoint;
    [SerializeField] PlayableDirector director;
    [SerializeField] BattleInfo battleInfo;


    private List<GameObject> currentCursor = new List<GameObject>();

 

    public BattleCharacter currentActor;
    public PlayerStorage infoStorage;
    public CharacterInventory playerInventory;
    public float minimumFadeTime = 1f;
    public static float DestroyTime = 1.4f;
    [SerializeField] Party HeroParty;
    [SerializeField] Party EnemyParty;
    private BattleState BattleState;
    public List<BattleCharacter> HeroPartyActors;
    public List<BattleCharacter> EnemyPartyActors;
    public List<BattleCharacter> Actors;
    int turn = 0;
    int numberOfturnsTotal = 0;
    int numberOfLoops = 0;


    public List<BattleItem> GetPlayerItems()
    {
        List<BattleItem> battleItems = new List<BattleItem>();

        foreach(InventoryItem item in playerInventory.myInventory)
        {
            if(item is BattleItem)
            {
                battleItems.Add((BattleItem)item);
            }
        }


        return battleItems;
    }

    public void SetControlText(string v)
    {
            battleControlText.text = v;
    }
    public void SetIndicationText(string v)
    {
            battleIndicationText.text = v;
        
    }


    public BattleCharacter GetActor()
    {
        return currentActor;
    }
    public bool NextActorIsSameTeam()
    {
        return Actors[GetNextRealTurnIndex()].GetTeam() == GetActor().GetTeam();
    }
    public bool NextActorCanAct()
    {

        return Actors[GetNextRealTurnIndex()].CanAct();
    }
    public bool IsEnemyTurn()
    {
        return GetActor().GetTeam() == TeamIndex.Enemy;
    }
    public bool NextActorIsPlayer()
    {
        return Actors[GetNextRealTurnIndex()].GetTeam() == TeamIndex.Player;
    }
    public int GetActorIndex(BattleCharacter source)
    {
        return GetPartyOf(source).IndexOf(source);
    }

    private int GetNextRealTurnIndex()
    {
        int firstTurn = turn;
        int nextturn;
        for (nextturn = turn; nextturn <= Actors.Count; nextturn++){


            if(nextturn < Actors.Count)
            {

                if (nextturn != firstTurn)
                {
                    if (!GetActor(nextturn).Entity.isDead)
                    {
                        return nextturn;
                    }
                }
            }
            else
            {

                nextturn = -1;
            }
        }
        return nextturn;
    }
    private int GetNextTurnIndex()
    {
        int nextturn = turn + 1;
        if (nextturn >= Actors.Count)
        {
            nextturn = 0;
        }
        return nextturn;
    }
    public List<BattleCharacter> GetPossibleTarget()
    {
        return GetPossibleTarget(GetActor().currentCommand);
    }
    public List<BattleCharacter> GetPossibleTarget(Skill a, BattleCharacter Source)
    {
        if (a == null)
        {
            return null;
        }
        Command c = a.GetCommandType();
        c.SetSource(Source);
        return GetPossibleTarget(c);
    }
    public List<BattleCharacter> GetPossibleTarget(Command c)
    {
        if (c  == null)
        {
            return new List<BattleCharacter>();
        }

        List<BattleCharacter> possibleTargets = new List<BattleCharacter>();
        TeamIndex sourceTeamIndex = c.Source.GetTeam();

        // Find all characters in the scene

        foreach (BattleCharacter character in Actors)
        {
            TeamIndex characterTeamIndex = character.GetTeam();
            if (c.CanBeTarget(character))
            {

                // Check if the command is friendly or not and add appropriate targets
                if ((c.friendliness == Friendliness.Friendly && characterTeamIndex == sourceTeamIndex) || (c.friendliness == Friendliness.Non_Friendly && characterTeamIndex != sourceTeamIndex)||(c.friendliness == Friendliness.Neutral))
                {
                    possibleTargets.Add(character);
                }
            }
        }

        return possibleTargets;
    }

    public List<BattleCharacter> GetCurrentTarget()
    {
        if (GetActor().currentCommand != null)
        {
            return GetActor().currentCommand.Target;
        }
        return new List<BattleCharacter>();
    }

    public BattleCharacter RandomActor(TeamIndex player)
    {
        foreach(BattleCharacter b in Actors)
        {
            if(b.GetTeam() == player)
            {
                return b;
            }
        }
        return null;
    }


    private BattleCharacter GetActor(int nextturn)
    {
        return nextturn < Actors.Count ? Actors[nextturn] : null;
    }
    public bool IsForcedTurn()
    {
        return GetActor() != Actors[turn];
    }


    private void Awake()
    {
        Singleton = this;
    }

    public void AddActor(BattleCharacter actor)
    {
        if (!Actors.Contains(actor))
        {
            Actors.Add(actor);
        }
    }

    private void Start()
    {
        SetupBattle();
    }

    public void SetupBattle()
    {
        ClearStage();

        SpawnEveryMember(HeroParty, TeamIndex.Player);
        SpawnEveryMember(EnemyParty, TeamIndex.Enemy);
        SpawnPartyCards();
        SetBattleInfo();

    }


    public void ClearStage()
    {

        foreach (BattleCharacter obj in Actors)
        {
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }
        Actors.Clear();
    }
    public void SetActorIndex(BattleCharacter source, int targetIndex)
    {
        List<BattleCharacter> party = GetPartyOf(source);

        if (party == null || !party.Contains(source))
        {
            Debug.LogError("The source character is not in the specified party.");
            return;
        }

        // Remove the source character from its current position
        party.Remove(source);

        // Ensure the target index is within the valid range
        targetIndex = Mathf.Clamp(targetIndex, 0, party.Count);

        // Insert the source character at the target index
        party.Insert(targetIndex, source);



        UpdateActorsList();

    }

    public void UpdateActorsList()
    {
        Actors = new List<BattleCharacter>(HeroPartyActors);
        Actors.AddRange(EnemyPartyActors);
    }
    public void SpawnEveryMember(Party party, TeamIndex index)
    {
        foreach (CharacterObject character in party.PartyMembers)
        {
            SpawnCharacter(character, index);
        }
    }
    public void InitializeBattle()
    {
        Actors.Sort((a, b) => b.Entity.Speed.CompareTo(a.Entity.Speed));
        SetActor();
        StartNewTurn();
    }

    public void SetBattleInfo()
    {
        if (battleInfo)
        {
            PlayOST();
            PlayCutscene();
        }

    }
    public void PlayCutscene()
    {
        ChangeState(new NothingState());


        director.GetComponent<BattleTimeline>().SetCutscene(battleInfo.CutsceneForDialogue);

        director.playableAsset = battleInfo.CutsceneToPlay;

        if (director.playableAsset == null || battleInfo.CutsceneForDialogue.RuntimeValue){
            Debug.Log("Direct Start");
            StartBattle();
       }
       else
        {

          Debug.Log("Cutscene Start");
          director.Play();
        }
    }

    public void PlayOST()
    {
        if (battleInfo.BattleMusic)
        {
            Debug.Log(battleInfo.BattleMusic);
            MusicPlayer.Singleton?.PlaySong(battleInfo.BattleMusic, true);
        }
    }

    public void ChangeState(BattleState state)
    {
      
        BattleState?.OnExit();
        BattleState = state;
        BattleState.OnEnter(this);
        if (DebugBattleStateText != null)
        {
            DebugBattleStateText.text = BattleState.GetType().ToString();
        }
    }
    private void SpawnPartyCards()
    {
        foreach(BattleCharacter actor in Actors)
        {
            if(actor.GetTeam() == TeamIndex.Player)
            {
                GameObject PartyCard = Instantiate(CardUIPrefab, CardUIContainer);
                PartyCard.GetComponent<PartyCard>().SetPlayerRef(actor);
            }
        }
    }


    public void ResetPartyCards()
    {
        foreach(Transform child in CardUIContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void Update()
    {
        BattleState.Handle();
    }

    public void SetCursor(BattleCharacter character, bool resetCursors = true)
    {
        if (resetCursors)
        {
            foreach(GameObject cursor in currentCursor)
            {
                Destroy(cursor);
            }
            currentCursor.Clear();
        }
        if (character)
        {
            
            GameObject cursor = Instantiate(CursorPrefab, character.transform.position + (Vector3.up * 3.1f), Quaternion.identity);
            cursor.GetComponent<Blink>().SetDefaultColor(TeamComponent.TeamColor(character.GetTeam()));
            currentCursor.Add(cursor);
        }
    }


    public Vector3 GetPosition(BattleCharacter battleCharacter)
    {
        Vector3 direction = Vector3.left;
        Vector3 basePosition = PlayerSpawnPoint.position;
        int layerOrder;
        List<BattleCharacter> currentParty = new List<BattleCharacter>();
        if (HeroPartyActors.Contains(battleCharacter))
        {
            currentParty = HeroPartyActors;
            direction = Vector3.left;
            basePosition = PlayerSpawnPoint.position;
        }
        else if (EnemyPartyActors.Contains(battleCharacter))
        {

            currentParty = EnemyPartyActors;
            direction = Vector3.right;
            basePosition = EnemySpawnPoint.position;
        }


        layerOrder = currentParty.IndexOf(battleCharacter)%2;
        basePosition += direction * currentParty.IndexOf(battleCharacter);
        basePosition += (Vector3.down / 2) * (layerOrder);


        return basePosition;

    }
    public void SpawnCharacter(CharacterObject characterObject, TeamIndex index)
    {
        Vector2 Position = PlayerSpawnPoint.position;
        int FlipIndex = 1;
        int layerOrder = 0;
        GameObject CharacterGameObject = Instantiate(BattlePrefab, Position, Quaternion.identity);
        BattleCharacter battleCharacterObject = CharacterGameObject.GetComponent<BattleCharacter>();
        battleCharacterObject.SetReference(characterObject);
        battleCharacterObject.Entity.OnDead += CheckTeams;
        Actors.Add(CharacterGameObject.GetComponent<BattleCharacter>());
        switch (index)
        {
            case TeamIndex.Player:

                CharacterGameObject.GetComponent<Entity>().LoadReference();
                HeroPartyActors.Add(battleCharacterObject);
                layerOrder = HeroPartyActors.IndexOf(battleCharacterObject) % 2;
                break;
            case TeamIndex.Enemy:
                CharacterGameObject.GetComponent<Entity>().LoadReferenceRefreshed();
                EnemyPartyActors.Add(battleCharacterObject);
                layerOrder = EnemyPartyActors.IndexOf(battleCharacterObject) % 2;
                FlipIndex = -1;
                break;

        }
        CharacterGameObject.transform.position = GetPosition(battleCharacterObject);
        battleCharacterObject.Flip(FlipIndex);
        battleCharacterObject.SetTeam(index);
        CharacterGameObject.name = characterObject.characterData.characterName + Actors.Count;

        CharacterGameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = layerOrder;








    }

    public List<BattleCharacter> GetPartyOf(BattleCharacter battleCharacter)
    {
        if (HeroPartyActors.Contains(battleCharacter))
        {
            return HeroPartyActors;
        }
        else if (EnemyPartyActors.Contains(battleCharacter))
        {

            return EnemyPartyActors;
        }
        return null;
    }


    public void OnBattleWon()
    {
        Debug.Log("Battle Won");
    }

    public void OnBattleLoss()
    {
        Debug.Log("Battle Loss");
    }
    public void CheckTeams()
    {
        if (!CheckTeamAlive(EnemyPartyActors))
        {
            EndBattle(true);
        }
        else if (!CheckTeamAlive(HeroPartyActors))
        {

            EndBattle(false);
        }
    }

    public IEnumerator StartBattleCountDown()
    {
        yield return new WaitForSeconds(.5f);
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(.3f);
        }
        InitializeBattle();
    }
    public void StartBattle()
    {
        StartCoroutine(StartBattleCountDown());
    }

    public void EndBattle(bool won)
    {
        StartCoroutine(EndBattleCountDown(won));
    }
    public bool CheckTeamAlive(Party party)
    {
        foreach (BattleCharacter character in Actors)
        {

            if (party.PartyMembers.Contains(character.GetReference()))
            {
                Debug.Log(character.name + " " + party.name);
                if (!character.Entity.isDead)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool CheckTeamAlive(List<BattleCharacter> party)
    {
        foreach (BattleCharacter character in party)
        {
                if (!character.Entity.isDead)
                {
                    return true;
                }
            
        }
        return false;
    }



    public IEnumerator EndBattleCountDown(bool battleWon)
    {
        Time.timeScale = .3f;
        ChangeState(new NothingState());
        foreach (BattleCharacter character in Actors)
        {
            if (character.GetTeam() == TeamIndex.Player)
            {

                character.Entity.Apply();
            }
            yield return null;
        }
        yield return new WaitForSeconds(.5f);


        if (battleWon)
        {
            OnBattleWon();
        }
        else
        {
            OnBattleLoss();
        }


        Time.timeScale = 1f;
        yield return new WaitForSeconds(3f);

        MoveToScene();



    }



    public void NextTurn()
    {
        turn = GetNextTurnIndex();
        if(turn == 0)
        {
            numberOfLoops++;
        }
        SetActor();
    }
    public void StartNewTurn()
    {
        numberOfturnsTotal++;
        if (GetActor().Entity.isDead)
        {
            GetActor().currentCommand = new DeadCommand();
            GetActor().currentCommand.SetSource(GetActor());
            ChangeState(new PerformActionState());
        }else if (GetActor().IsPlayerTeam())
        {

            ChangeState(new ChoosingActionState());
        }
        else
        {
            GetActor().currentCommand = GetActor().CreateCommand();
            GetActor().currentCommand.SetSource(GetActor());
            GetActor().currentCommand.SetTarget(GetRandomTargets());
            ChangeState(new PerformActionState());

        }

    }

    private void SetActor()
    {
        currentActor = Actors[turn];
    }

    public void SetActor(BattleCharacter bc)
    {
        if (Actors.Contains(bc))
        {
            currentActor = Actors[Actors.IndexOf(bc)];
        }
        
    }

    public void MoveToScene()
    {
        if (infoStorage)
        {

            if (infoStorage.sceneName != SceneManager.GetActiveScene().name)
            {
                infoStorage.forceNextChange = true;
                StartCoroutine(FadeCoroutine(infoStorage.sceneName));
            }



        }

    }
    public IEnumerator FadeCoroutine(string scene)
    {

        if (!FadeScreen.fading)
        {
            FadeScreen.StartTransition(true, Color.black, .5f);
        }
        yield return new WaitForSeconds(minimumFadeTime * 2);


        ClearStage();




        yield return new WaitForSeconds(minimumFadeTime / 2);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        if (!FadeScreen.fading)
        {
            FadeScreen.StartTransition(false, Color.black, .5f);
        }
        yield return null;
    }

    private List<BattleCharacter> GetRandomTargets()
    {
        List<BattleCharacter> targets = new List<BattleCharacter>();
        targets.Add(GetPossibleTarget()[Random.Range(0, GetPossibleTarget().Count)]);

        return targets;
    }
}