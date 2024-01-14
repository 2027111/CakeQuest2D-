using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBaseState : State
{
    public float duration;
    protected bool shouldCombo;
    protected int attackIndex;
    private float attackPressedTimer = 0;
    protected bool hasHit = false;
    protected bool conserveVelocity = false;
    protected AttackData currentData;
    protected HitBoxInfo currentHitBox;
    protected PrefabInfo currentPrefab;
    protected int frame = 0;
    protected float framerate = 12;
    protected int lastFrame = -1;
    protected Collider2D hitCollider;
    private List<Collider2D> collidersDamaged;
    private List<ForceEvents> forceEvents = new List<ForceEvents>();
    protected AttackData nextData;
    AnimatorOverrideController originalController;
    AnimationClip originalClip;

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        collidersDamaged = new List<Collider2D>();
        forceEvents = new List<ForceEvents>();
        hitCollider = stateMachine.GetComponent<BattleCharacter>().hitbox;
        cc.gameObject.layer = 9;
        animator.SetBool("IsAttacking", true);
        animator.SetTrigger("Attack");
    }

    public void OnEnter(StateMachine _stateMachine, AttackData data)
    {
        base.OnEnter(_stateMachine);

        if (!data)
        {
            stateMachine.SetNextStateToMain();

        }
        else if(data.GetType() == typeof(AttackData))
        {

            collidersDamaged = new List<Collider2D>();
            hitCollider = stateMachine.GetComponent<BattleCharacter>().hitbox;
            currentData = data;
            duration = currentData.animation.length;
            framerate = currentData.animation.frameRate;
            cc.attackPlacement = currentData.attackPlacement;
            animator.SetFloat("Weapon.Active", 0);
            animator.SetFloat("AttackWindow.Open", 0);
            PlaySFXs();
            if (!currentData.conserveVelocity)
            {
                cc.rb.velocity = Vector3.zero;
            }
            animator.SetBool("IsAttacking", true);


            cc.entity.AddToMana(-currentData.manaCost);


            ApplyAttackAnimationOverride();







            animator.SetTrigger("Attack");
        }

    }

    private void PlaySFXs()
    {

        if (currentData.SoundEffect)
        {
            cc.PlaySFX(currentData.SoundEffect);
        }
        if (currentData.VoiceLine)
        {
            cc.PlayVoiceclip(currentData.VoiceLine);
        }
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
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, currentData.animation));
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
                if (!collidersDamaged.Contains(collidersToDamage[i]))
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
                            hasHit = true;
                        collidersDamaged.Add(collidersToDamage[i]);
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
        animator.SetFloat("Weapon.Active", 0);
        animator.SetFloat("AttackWindow.Open", 0);
        cc.SetHitBox(false);
        cc.attackPlacement = AttackPlacement.NONE;
        collidersDamaged = new List<Collider2D>();
        if (!hasHit)
        {
            cc.CooldownAttack(.3f);
        }
    }




    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public void DoAttack()
    {
        if (cc.canMove && cc.canAttack && cc.entity.CheckManaCost(cc.GetCurrentAttack()) && currentData != cc.GetCurrentAttack())
        {
            attackPressedTimer = 2;
            nextData = cc.GetCurrentAttack();
        }
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
            if (currentData.hitboxes.Count > 0)
            {
                currentHitBox = currentData.GetHitBoxByFrame(frame);
                if (currentHitBox != null)
                {
                    if (frame > lastFrame)
                    {
                        if (currentHitBox.resetCollisions && currentHitBox.IsFirstFrame(frame))
                        {
                            Debug.Log("Reset Collisions");
                            collidersDamaged = new List<Collider2D>();
                        }
                        animator.SetFloat("Weapon.Active", 1);
                        cc.SetHitBox(true, currentHitBox);
                        Attack();
                    }
                }
            }
            if (currentData.prefabs.Count > 0)
            {
                currentPrefab = currentData.GetPrefabByFrame(frame);
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
            if (currentData.forceEvents.Count > 0)
            {
                ForceEvents force = currentData.GetForceEventsByFrame(frame);
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

            if (frame >= currentData.startOpenFrame && hasHit)
            {
                if (animator.GetFloat("AttackWindow.Open") == 0)
                {
                    //cc.OnAttackPressed += DoAttack;
                    animator.SetFloat("AttackWindow.Open", 1);
                }
            }
        }

        if (attackPressedTimer > 0 && animator.GetFloat("AttackWindow.Open") > 0 && nextData)
        {
            Debug.Log(attackPressedTimer + " " + nextData);
            stateMachine.SetNextState(new MeleeBaseState(), nextData);
        }

        attackPressedTimer -= Time.deltaTime;




        if (fixedtime > duration)
        {
            stateMachine.SetNextStateToMain();

        }

        lastFrame = frame;
    }
}
