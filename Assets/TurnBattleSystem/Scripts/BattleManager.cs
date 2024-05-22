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
    [SerializeField] TMP_Text text;



    public GameObject BattlePrefab;
    [SerializeField] GameObject CursorPrefab;
    [SerializeField] GameObject CardUIPrefab;
    [SerializeField] Transform CardUIContainer;
    [SerializeField] Transform PlayerSpawnPoint;
    [SerializeField] Transform EnemySpawnPoint;
    [SerializeField] PlayableDirector director;
    [SerializeField] BattleInfo battleInfo;
    private List<GameObject> currentCursor = new List<GameObject>();

    public PlayerStorage infoStorage;
    public float minimumFadeTime = 1f;
    public static float DestroyTime = 1.4f;
    [SerializeField] Party HeroParty;
    [SerializeField] Party EnemyParty;
    private BattleState BattleState;
    public List<BattleCharacter> HeroPartyActors;
    public List<BattleCharacter> EnemyPartyActors;
    public List<BattleCharacter> Actors;
    int EnemyIndex = 0;
    int PlayerIndex = 0;
    int turn = 0;
    public BattleCharacter GetActor()
    {
        return Actors[turn];
    }

    public List<BattleCharacter> GetPossibleTarget()
    {
        return GetPossibleTarget(GetActor().currentCommand);
    }

    public List<BattleCharacter> GetPossibleTarget(Command c)
    {
        if (c  == null)
        {
            return null;
        }

        List<BattleCharacter> possibleTargets = new List<BattleCharacter>();
        TeamComponent sourceTeamComponent = c.Source.GetComponent<TeamComponent>();

        // Find all characters in the scene

        foreach (BattleCharacter character in Actors)
        {
            TeamComponent characterTeamComponent = character.GetComponent<TeamComponent>();
            if (c.CanBeTarget(character))
            {

                // Check if the command is friendly or not and add appropriate targets
                if (c.friendly)
                {
                    if (characterTeamComponent.teamIndex == sourceTeamComponent.teamIndex)
                    {
                        possibleTargets.Add(character);
                    }
                }
                else
                {
                    if (characterTeamComponent.teamIndex != sourceTeamComponent.teamIndex)
                    {
                        possibleTargets.Add(character);
                    }
                }
            }
        }

        return possibleTargets;
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

        EnemyIndex = 0;
        PlayerIndex = 0;
        foreach (BattleCharacter obj in Actors)
        {
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }
        Actors.Clear();
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
        Actors.Sort((a, b) => b.Speed.CompareTo(a.Speed));

        turn = Actors.Count;
        NextTurn();
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
        if (text != null)
        {
            text.text = BattleState.GetType().ToString();
        }
    }
    private void SpawnPartyCards()
    {
        foreach(BattleCharacter actor in Actors)
        {
            if(actor.GetComponent<TeamComponent>().teamIndex == TeamIndex.Player)
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
            
            GameObject cursor = Instantiate(CursorPrefab, character.transform.position + (Vector3.up * 2.1f), Quaternion.identity);
            cursor.GetComponent<Blink>().SetDefaultColor(TeamComponent.TeamColor(character.GetTeam()));
            currentCursor.Add(cursor);
        }
    }
    public void SpawnCharacter(CharacterObject characterObject, TeamIndex index)
    {
        Vector2 Position = PlayerSpawnPoint.position;
        int FlipIndex = 1;
        int layerOrder = 0;
        if (index == TeamIndex.Player)
        {
            Position = PlayerSpawnPoint.position;
            Position += Vector2.left * PlayerIndex;
            layerOrder = PlayerIndex % 2;
            Position += (Vector2.down / 2) * (layerOrder);

            PlayerIndex++;
        }
        else if (index == TeamIndex.Enemy)
        {

            Position = EnemySpawnPoint.position;
            Position += Vector2.right * EnemyIndex;
            layerOrder = EnemyIndex % 2;
            Position += (Vector2.down / 2) * (layerOrder);
            FlipIndex = -1;
            EnemyIndex++;
        }
        GameObject CharacterGameObject = Instantiate(BattlePrefab, Position, Quaternion.identity);

        CharacterGameObject.GetComponent<BattleCharacter>().SetReference(characterObject);
        CharacterGameObject.GetComponent<Entity>().OnDead += CheckTeams;
        CharacterGameObject.GetComponent<BattleCharacter>().Flip(FlipIndex);
        CharacterGameObject.GetComponent<TeamComponent>().teamIndex = index;
        CharacterGameObject.name = characterObject.characterData.characterName;
        CharacterGameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = layerOrder;
        Actors.Add(CharacterGameObject.GetComponent<BattleCharacter>());
        switch (index)
        {
            case TeamIndex.Player:

                CharacterGameObject.GetComponent<Entity>().LoadReference();
                HeroPartyActors.Add(CharacterGameObject.GetComponent<BattleCharacter>());
                break;
            case TeamIndex.Enemy:
                CharacterGameObject.GetComponent<Entity>().LoadReferenceRefreshed();
                EnemyPartyActors.Add(CharacterGameObject.GetComponent<BattleCharacter>());
        break;

        }




      


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

        turn++;
        if(turn >= Actors.Count)
        {
            turn = 0;
        }

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
