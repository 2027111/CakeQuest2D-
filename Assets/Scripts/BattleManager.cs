using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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



    [SerializeField] Party HeroParty;
    [SerializeField] Party EnemyParty;
    [SerializeField] Transform Canvas;
    [SerializeField] BattleInfo battleInfo;
    List<GameObject> characters = new List<GameObject>();
    [SerializeField] GameObject CharacterUIPrefab;
    [SerializeField] GameObject MoveIndicatorPrefab;
    [SerializeField] GameObject MoveIndicator;
    [SerializeField] Transform HealthUIContainer;
    [SerializeField] CinemachineTargetGroup targetGroup;
    [SerializeField] PlayableDirector director;
    [SerializeField] Transform PlayerSpawnPoint;
    [SerializeField] Transform EnemySpawnPoint;
    [SerializeField] AudioSource audioSource;
    public bool debug = true;

    public GameObject BattlePrefab;
    int EnemyIndex = 0;
    int PlayerIndex = 0;

    bool pause = false;

    public PlayerStorage infoStorage;
    

    public float minimumFadeTime = 1f;
    public static float DestroyTime = 1.4f;


    public BattleCharacter Selected = null;


    // Start is called before the first frame update

    private void Awake()
    {
        Singleton = this;
    }
    public void SpawnMoveIndicator(MoveSetInfos msi)
    {
        if(MoveIndicator != null)
        {
            Destroy(MoveIndicator);
        }
        MoveIndicator = Instantiate(MoveIndicatorPrefab, Canvas);
        MoveIndicator.GetComponent<MoveSetIndicator>().SetMoveSetInfos(msi);
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
        foreach (Projectile proj in Projectile.projectilesSpawned)
        {
            proj.SetOff();
        }
        yield return new WaitForSeconds(minimumFadeTime*2);


        ClearStage();




        yield return new WaitForSeconds(minimumFadeTime/2);
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




    private void Start()
    {
        SetupBattle();
        if (!FadeScreen.fading)
        {
            FadeScreen.StartTransition(false, Color.black, .5f);
        }
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
       
            
            director.GetComponent<BattleTimeline>().SetCutscene(battleInfo.CutsceneForDialogue);
        
            director.playableAsset = battleInfo.CutsceneToPlay;
        
        if(director.playableAsset == null || battleInfo.CutsceneForDialogue.RuntimeValue)
        {
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
            audioSource.clip = battleInfo.BattleMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    public void SetupHeroUI(CharacterObject character)
    {
        HealthBarUI HealthUI = Instantiate(CharacterUIPrefab, HealthUIContainer).GetComponent<HealthBarUI>();
        HealthUI.SetPlayerRef(character);
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
        if (!CheckTeamAlive(EnemyParty))
        {
            EndBattle(true);
        }else if (!CheckTeamAlive(HeroParty))
        {

            EndBattle(false);
        }
    }


    public void EndBattle(bool won)
    {
        StartCoroutine(EndBattleCountDown(won));
    }


    public bool CheckTeamAlive(Party party)
    {
        if (party.MainCharacter)
        {
            if (!party.MainCharacter.isDead)
            {
                return true;
            }
        }

        foreach (CharacterObject character in party.PartyMembers)
        {
            if (!character.isDead)
            {
                return true;
            }
        }
        return false;
    }
    public void SetupBattle()
    {
        ClearStage();
        SpawnCharacter(HeroParty.MainCharacter, TeamIndex.Player ,true);

        SpawnEveryMember(HeroParty, TeamIndex.Player);
        SpawnEveryMember(EnemyParty, TeamIndex.Enemy);
        SetBattleInfo();

    }

    public IEnumerator StartBattleCountDown()
    {
        yield return new WaitForSeconds(.5f);
        for (int i = 0; i < 3; i++)
        {
            Debug.Log(3 - i);
            yield return new WaitForSeconds(.3f);
        }

        BattleState();
    }
    public void StartBattle()
    {
        StartCoroutine(StartBattleCountDown());
    }


    public void BattleState()
    {

        foreach (GameObject character in characters)
        {
            if (character.GetComponent<StateMachine>())
            {
                if (!character.GetComponent<Entity>().characterObject.isDead)
                {

                    character.GetComponent<StateMachine>().SetNextStateToMain();
                }
            }
        }
    }



    public IEnumerator EndBattleCountDown(bool battleWon)
    {
        Time.timeScale = .3f;
        foreach (GameObject character in characters)
        {
            if (!character.GetComponent<Entity>().characterObject.isDead)
            {
                character.GetComponent<Controller>().Disable();
            }
            yield return null;
        }
        yield return new WaitForSeconds(.5f);

        Time.timeScale = 1f;

        if (battleWon)
        {
            OnBattleWon();
        }
        else
        {
            OnBattleLoss();
        }
        

        yield return new WaitForSeconds(3f);

        MoveToScene();



    }

    public void ClearStage()
    {

        EnemyIndex = 0;
        PlayerIndex = 0;
        foreach (GameObject obj in characters)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        characters.Clear();
        foreach (Transform obj in HealthUIContainer)
        {
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }
    }

    public void SpawnEveryMember(Party party, TeamIndex index)
    {
        foreach(CharacterObject character in party.PartyMembers)
        {
            SpawnCharacter(character, index);
        }
    }

    public void OnSpecialMove()
    {

    }


  
    public void SpawnCharacter(CharacterObject characterObject, TeamIndex index,bool Playable = false)
    {
        Vector2 Position = PlayerSpawnPoint.position;
        int FlipIndex = 1;
        if (index == TeamIndex.Player)
        {
            Position = PlayerSpawnPoint.position;
            Position += Vector2.left * PlayerIndex;
            PlayerIndex++;
            SetupHeroUI(characterObject);
        }
        else if(index == TeamIndex.Enemy)
        {

            Position = EnemySpawnPoint.position;
            Position += Vector2.right * EnemyIndex;
            FlipIndex = -1;
            characterObject.Revitalize();
            EnemyIndex++;
        }
        GameObject CharacterGameObject = Instantiate(BattlePrefab, Position, Quaternion.identity);

        CharacterGameObject.GetComponent<Entity>().characterObject = characterObject;
        CharacterGameObject.GetComponent<Entity>().OnDead += CheckTeams;
        CharacterGameObject.GetComponent<BattleCharacter>().Flip(FlipIndex);
        CharacterGameObject.GetComponent<BattleCharacter>().SetColliderInformation(characterObject);
        CharacterGameObject.GetComponent<BattleCharacter>().SetAnimatorController(characterObject.animationController);
        CharacterGameObject.GetComponent<BattleCharacter>().alm.SetAttackList(characterObject.AttackList);
        CharacterGameObject.GetComponent<TeamComponent>().teamIndex = index;
        CharacterGameObject.name = characterObject.characterData.characterName;
        characters.Add(CharacterGameObject);



        if (characterObject.isDead)
        {

            CharacterGameObject.GetComponent<StateMachine>().SetNextState(new DeadState());
        }
        else
        {

            CharacterGameObject.GetComponent<StateMachine>().SetNextState(new EntranceState());
        }


        if (!Playable)
        {
            Component[] componentsToRemove = {
            CharacterGameObject.GetComponent<Controller>(),
            CharacterGameObject.GetComponent<PlayerInput>()
            };

            foreach (var component in componentsToRemove)
            {
                if (component != null)
                {
                    Destroy(component);
                }
            }
            CharacterGameObject.AddComponent<AIController>();
            if (debug)
            {
                CharacterGameObject.GetComponent<AIController>().Disable();
            }
        }
        else
        {
            CharacterGameObject.tag = "Player";
        }

        if (targetGroup)
        {
            targetGroup.AddMember(CharacterGameObject.transform, Playable?10:5, 5);
        }



        
    }



}
