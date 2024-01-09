using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;






[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(TeamComponent))]
[RequireComponent(typeof(AttackListManager))]
[RequireComponent(typeof(Entity))]
public class BattleCharacter : MonoBehaviour
{


    public delegate void EventHandler();
    public delegate void MovementHandler(Vector2 movement);
    [Header("Inputs")]
    public EventHandler OnJumpPressed;
    public EventHandler OnAttackPressed;
    public EventHandler OnJumpRelease;
    public EventHandler OnAttackRelease;
    public MovementHandler OnMovement;
    public bool jump;
    public bool attack;
    public Vector2 direction = Vector2.zero;

    [Space]
    private StateMachine meleeStateMachine;

    [Space]
    [Header("Stats")]
    public float speed = 10;
    public float jumpForce = 50;
    private float currentSpeed = 0f;
    public float acceleration = 10f;
    public float decceleration = 0.3f;
    public int doubleJumpAmount = 1;
    public float doubleJumpCoolDown = 0.3f;
    public float doubleJumpCoolDownAmount = 0.3f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 5f;
    public float invincibilityTime = 1.2f;
    public int side = 1;
    [Space]
    [Header("Booleans")]
    public bool canMove;
    public bool canAttack = true;
    public bool groundTouch;
    private bool Intangibility = false;

    [Space]
    public AttackPlacement attackPlacement = AttackPlacement.NONE;
    [Space]
    [SerializeField] public Collider2D hitbox;
    [SerializeField] public Collider2D hurtbox;
    [SerializeField] public Animator animator;
    [SerializeField] public GameObject Graphics;
    [SerializeField] public CinemachineVirtualCamera virtualCamera;
    [SerializeField] public GameObject Hiteffect;
    [SerializeField] private Collision coll;
    [SerializeField] public Rigidbody2D rb;



    [SerializeField] public AttackListManager alm;
    [SerializeField] public Entity entity;

    [SerializeField] public SpriteRenderer characterSprite;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        alm = GetComponent<AttackListManager>();
        rb = GetComponent<Rigidbody2D>();
        characterSprite = Graphics.GetComponentInChildren<SpriteRenderer>();
        coll = GetComponent<Collision>();
        meleeStateMachine = GetComponent<StateMachine>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }


    public void SetAnimatorController(AnimatorOverrideController aoc)
    {
        if (aoc)
        {
            animator.runtimeAnimatorController = aoc;
        }
    }
    public void StartInvincibilityFrames()
    {
        StartCoroutine(StartInvincibility());
    }

    // Update is called once per frame
    void Update()
    {
        if (coll.onGround && !groundTouch)
        {
            groundTouch = true;
            doubleJumpAmount = 1;
        }

        if (!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }

        if (!coll.onGround && doubleJumpCoolDownAmount > 0)
        {
            doubleJumpCoolDownAmount -= Time.deltaTime;
        }

    }
    public AnimatorOverrideController GetController()
    {
        return entity.characterObject.animationController ? entity.characterObject.animationController : new AnimatorOverrideController(animator.runtimeAnimatorController);
    }
    public void OnJumpPress()
    {
        OnJumpPressed?.Invoke();
        jump = true;
    }

    public void OnAttackPress()
    {
        OnAttackPressed?.Invoke();
        attack = true;
    }


    public void OnJumpLetGo()
    {
        OnJumpRelease?.Invoke();
        jump = false;
    }

    public void OnAttackLetGo()
    {
        OnAttackRelease?.Invoke();
        attack = false;
    }

    public void SetMove(Vector2 Movement)
    {
        direction = Movement;
        OnMovement?.Invoke(Movement);
    }


    private void OnDrawGizmos()
    {

        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube((Vector2)hitbox.transform.position + (new Vector2(hitbox.offset.x * Graphics.transform.localScale.x, hitbox.offset.y)), ((BoxCollider2D)hitbox).size);
    }

    public void ResetMovement()
    {
        currentSpeed = 0;
    }
    public void Flip(int side)
    {

        if (Graphics.transform.localScale.x == side)
                return;


        Graphics.transform.localScale = new Vector3(side, Graphics.transform.localScale.y, Graphics.transform.localScale.z);
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    public void CooldownAttack(float time)
    {
        StartCoroutine(AttackCooldown(time));
    }

    public AttackData GetCurrentAttack()
    {
        AttackPlacement ap = AttackPlacement.NONE;
            float x = direction.x;
            float y = direction.y;
                if (Mathf.Abs(x) > 0.2f)
                {
                    ap = AttackPlacement.SLIGHT;
                }
                else if (y < -0.2f)
                {
                    ap = AttackPlacement.DLIGHT;

                }
                else
                {
                    ap = AttackPlacement.NLIGHT;

                }
            
        return GetAttack(ap);
    }

    public void Walk(Vector2 dir)
    {

        if (!canMove)
            return;
       
        float targetSpeed = dir.magnitude * speed;

        float accel = currentSpeed < targetSpeed ? acceleration : decceleration;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accel * Time.deltaTime);

        rb.velocity = new Vector2(dir.x * currentSpeed, rb.velocity.y);

    }
    public void Jump(Vector2 dir, bool wall)
    {

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;
    }

    public void SetHitBox(bool turnOn, Vector2? size = null, Vector2? offset = null)
    {

        Vector2 nOffSet = offset != null && turnOn ? (Vector2)offset : Vector2.zero;
        Vector2 nSize = size != null && turnOn ? (Vector2)size : Vector2.zero;
        if (turnOn)
        {
            hitbox.gameObject.SetActive(true);
            hitbox.offset = nOffSet;
            ((BoxCollider2D)hitbox).size = nSize;
        }
        else
        {
            hitbox.gameObject.SetActive(false);
            hitbox.offset = nOffSet;
            ((BoxCollider2D)hitbox).size = nSize;
        }
    }

    public void SetHitBox(bool turnOn, HitBoxInfo currentHitBox)
    {
        Vector2 nOffSet = currentHitBox != null && turnOn ? (Vector2)currentHitBox.offset : Vector2.zero;
        Vector2 nSize = currentHitBox != null && turnOn ? (Vector2)currentHitBox.size : Vector2.zero;

        if (turnOn)
        {
            hitbox.gameObject.SetActive(true);
            hitbox.offset = nOffSet;
            ((BoxCollider2D)hitbox).size = nSize;
        }
        else
        {
            hitbox.gameObject.SetActive(false);
            hitbox.offset = nOffSet;
            ((BoxCollider2D)hitbox).size = nSize;
        }
    }

    public AttackData GetAttack()
    {
        return alm.AttackList[UnityEngine.Random.Range(0, alm.AttackList.Count)];
    }
    public AttackData GetAttack(AttackPlacement ap)
    {
        return alm.GetAttackMatching(ap);
    }


    
    public IEnumerator StartInvincibility()
    {
        int defaultLayer = gameObject.layer;
        Color NormalColor = characterSprite.color;
        Debug.Log("Invinc");
        if (!Intangibility)
        {
            Intangibility = true;
            gameObject.layer = 8;
            float duration = 0;
            bool opacity = false;
            float lastduration = duration;

            yield return new WaitForSeconds(0.4f);
            while (duration < invincibilityTime)
            {
                duration += Time.deltaTime;
                if(duration-lastduration >= invincibilityTime / 10)
                {
                    lastduration = duration;
                    opacity = !opacity;
                }
                yield return null;
                Color color = NormalColor;
                color.a = opacity ? 1 : 0;
                characterSprite.color = color;
            }
            NormalColor.a = 1;
            characterSprite.color = NormalColor;
            gameObject.layer = defaultLayer;
            Intangibility = false;








        }

        
    }

    public IEnumerator AttackCooldown(float time)
    {
        canAttack = false;
        yield return new WaitForSeconds(time);
        canAttack = true;
    }
}
