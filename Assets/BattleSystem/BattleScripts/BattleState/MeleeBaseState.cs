using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBaseState : AttackState
{
    public float duration;
    protected bool shouldCombo;
    protected int attackIndex;
    private float attackPressedTimer = 0;
    private float attackBetweener = 0.3f;
    private bool AttackWindowOpen = false;
    protected bool hasHit = false;
    protected HitBoxInfo currentHitBox;
    protected PrefabInfo currentPrefab;
    protected int frame = 0;
    protected float framerate = 12;
    protected int lastFrame = -1;
    protected Collider2D hitCollider;
    private List<GameObject> collidersDamaged;
    private List<ForceEvents> forceEvents = new List<ForceEvents>();
    protected MoveData nextData;
    AnimatorOverrideController originalController;
    AnimationClip originalClip;

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
    }

    public void OnEnter(StateMachine _stateMachine, AttackData data)
    {
        base.OnEnter(_stateMachine, data);

            collidersDamaged = new List<GameObject>();
            forceEvents = new List<ForceEvents>();
            hitCollider = stateMachine.GetComponent<BattleCharacter>().hitbox;
            cc.gameObject.layer = 9;
            SetCurrentData();
            animator.SetBool("IsAttacking", true);


            ApplyAttackAnimationOverride();







            animator.SetTrigger("Attack");
        

    }

    public void SetCurrentData()
    {
        duration = ((AttackData)currentData).animation.length;
        framerate = ((AttackData)currentData).animation.frameRate;
        cc.attackPlacement = currentData.attackPlacement;
    }



    private void ApplyAttackAnimationOverride()
    {
        originalController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        foreach (var a in originalController.animationClips)
        {
            if (a.name == "Attack_Temp")
            {
                originalClip = a;
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, ((AttackData)currentData).animation));
            }
        }
        originalController.ApplyOverrides(anims);
        animator.runtimeAnimatorController = originalController;
    }
    protected void Attack()
    {
        Collider2D[] collidersToDamage = new Collider2D[15];
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        filter.layerMask = stateMachine.hitboxMask;
        filter.useLayerMask = true;
        int colliderCount = Physics2D.OverlapCollider(hitCollider, filter, collidersToDamage);
        Vector2 firstPosition = stateMachine.transform.position;
        for (int i = 0; i < colliderCount; i++)
        {
            if (collidersToDamage[i].gameObject != cc.GetComponent<Collider2D>().gameObject)
            {
                if (!collidersDamaged.Contains(collidersToDamage[i].gameObject))
                {
                    TeamComponent hitTeamComponent = collidersToDamage[i].GetComponent<TeamComponent>();
                    if (hitTeamComponent)
                    {
                        if(hitTeamComponent.teamIndex != cc.GetComponent<TeamComponent>().teamIndex)
                        {

                        if (collidersToDamage[i].GetComponent<Entity>())
                        {
                            int damage = stateMachine.GetComponent<Entity>().characterObject.AttackDamage + Random.Range(-5, 5);
                            Entity entity = collidersToDamage[i].GetComponent<Entity>();
                                entity.TakeDamage(damage, stateMachine.GetComponent<Entity>(), currentHitBox);
                                entity.OnDamageTaken?.Invoke(-damage, stateMachine.GetComponent<BattleCharacter>());
                        }
                            Debug.Log(collidersToDamage[i].name + " -> Enemy has taken Damage");

                            if (!hasHit)
                            {
                                firstPosition = collidersToDamage[i].transform.position + Vector3.up;
                                currentData.SpawnHitEffect(firstPosition);
                            }
                            if(hasHit == false)
                            {

                                hasHit = true;
                                cc.OnAttackPressed += DoAttack;
                            }
                        collidersDamaged.Add(collidersToDamage[i].gameObject);
                        }
                    }
                }
            }
        }
    }
    public override void OnExit()
    {
        base.OnExit();
        animator.runtimeAnimatorController = cc.GetController();
        animator.SetBool("IsAttacking", false);
        AttackWindowOpen = false;
        cc.SetHitBox(false);
        cc.attackPlacement = AttackPlacement.NONE;
        collidersDamaged = new List<GameObject>();

        if (hasHit)
        {
            cc.OnAttackPressed -= DoAttack;
        }
        if (!hasHit)
        {
            cc.CooldownAttack(.8f);
        }
    }

    public override void DoMoveset(bool special = false)
    {
        Debug.Log(cc.attackPlacement);
        if (!nextData)
        {

        if(cc.attackPlacement == currentData.attackPlacement)
        {
            if (((AttackData)currentData).nextMovePart)
            {
                nextData = ((AttackData)currentData).nextMovePart;
            }
        }
        else
        {



            MoveData nextMove = cc.GetCurrentAttack(special);
            if(nextMove.GetType() == typeof(AttackData))
            {
    
                if (cc.canMove && cc.canAttack && nextMove && cc.entity.CheckManaCost(nextMove))
               {
                   if (nextMove.grounded && !cc.groundTouch)
                    {
                        return;
                    }
    
                   nextData = nextMove;
                }
            }
            }
        }
        attackPressedTimer = attackBetweener;

        if (nextData)
        {
            Debug.Log($"{nextData.name} {nextData.attackPlacement}");
        }
    }


    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }



    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
    }

    public AnimatorClipInfo[] GetAnimationClips()
    {
        AnimatorClipInfo[] animationClip = animator.GetCurrentAnimatorClipInfo(0);
        return animationClip;
    }

    public int GetAnimationClipFrame(AnimatorClipInfo animationClip)
    {
        frame = Mathf.FloorToInt(animationClip.weight * (animationClip.clip.length * animationClip.clip.frameRate));
        return frame;
    }

    public int GetCurrentAnimFrame()
    {
        AnimatorClipInfo[] animationClips = GetAnimationClips();
        if (animationClips.Length > 0)
        {
            AnimatorClipInfo clipInfo = animationClips[0];
            float animTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            float animFrame = animTime * clipInfo.clip.frameRate * clipInfo.clip.length;
            frame = Mathf.FloorToInt(animFrame);
            return frame;
        }
        return -1;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        frame = GetCurrentAnimFrame();
        if (currentData)
        {
            if (((AttackData)currentData).hitboxes.Count > 0)
            {
                currentHitBox = ((AttackData)currentData).GetHitBoxByFrame(frame);
                if (currentHitBox != null)
                {
                    if (frame > lastFrame)
                    {
                        if (currentHitBox.resetCollisions && currentHitBox.IsFirstFrame(frame))
                        {
                            Debug.Log("Reset Collisions");
                            collidersDamaged = new List<GameObject>();
                        }
                        cc.SetHitBox(true, currentHitBox);
                        Attack();
                    }
                }
            }
            if (((AttackData)currentData).prefabs.Count > 0)
            {
                currentPrefab = ((AttackData)currentData).GetPrefabByFrame(frame);
                if (currentPrefab != null)
                {
                    if (frame > lastFrame)
                    {


                        Vector3 defaultPosition = currentPrefab.DefaultSpawnPosition;
                        defaultPosition.x *= cc.Graphics.transform.localScale.x;
                        GameObject proj = GameObject.Instantiate(currentPrefab.AttackPrefab, cc.transform.position + defaultPosition, Quaternion.identity);
                        proj.GetComponent<Projectile>().SetDirection(cc.Graphics.transform.localScale.x);
                        proj.GetComponent<Projectile>().SetDuration(currentPrefab.durationInFrame);
                        proj.GetComponent<Projectile>().SetOwner(cc);
                    }
                }
            }
            if (((AttackData)currentData).forceEvents.Count > 0)
            {
                ForceEvents force = ((AttackData)currentData).GetForceEventsByFrame(frame);
                if (force != null && frame > lastFrame)
                {
                    if (!forceEvents.Contains(force))
                    {

                    if (force.resetsVel)
                    {
                        cc.rb.velocity = Vector2.zero;
                    }
                    Vector2 rbForce = new Vector2(cc.side * force.direction.x, force.direction.y).normalized;
                    cc.rb.AddForce(rbForce * force.force, force.forceMode);
                    forceEvents.Add(force);
                    }
                }
            }

            if (frame >= ((AttackData)currentData).startOpenFrame && hasHit)
            {
                if (!AttackWindowOpen)
                {
                    AttackWindowOpen = true;
                }
            }
        }

        if (attackPressedTimer > 0 &&  AttackWindowOpen && nextData)
        {
            Debug.Log(attackPressedTimer + " " + nextData);
            stateMachine.SetNextState(new AttackState(), nextData);
        }

        attackPressedTimer -= Time.deltaTime;




        if (fixedtime > duration)
        {
            stateMachine.SetNextStateToMain();

        }

        lastFrame = frame;
    }
}
