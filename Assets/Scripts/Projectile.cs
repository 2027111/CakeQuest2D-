using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{


    public static List<Projectile> projectilesSpawned = new List<Projectile>();
    public Vector2 initialVelocity;
    public int Damage = 10;
    Vector2 newVelocity;
    public float timePassed;
    public float duration = 0;
    public int direction = 1;
    public bool constantVelocity = false;
    public bool gravity = false;
    public BattleCharacter Owner;
    private List<GameObject> collidersDamaged = new List<GameObject>();
    bool On = true;
    Rigidbody2D rb2D;
    BoxCollider2D hitbox;
    public bool DestroyOnContact;    

    private void Start()
    {
        projectilesSpawned.Add(this);
        newVelocity = initialVelocity;
        newVelocity.x *= direction;
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.AddForce(newVelocity);
        hitbox = GetComponent<BoxCollider2D>();
        GetComponent<SpriteRenderer>().flipX = direction == -1 ? true : false;
        rb2D.gravityScale = gravity ? 1 : 0;
        collidersDamaged = new List<GameObject>();
        Destroy(gameObject, duration);
    }



    public void Update()
    {
        if (constantVelocity)
        {

            rb2D.AddForce(newVelocity * Time.deltaTime);
        }
        Attack();
    }
    private void OnDestroy()
    {
        projectilesSpawned.Remove(this);
    }


    protected void Attack()
    {
        if (On)
        {

        if(Owner == null)
        {
            Destroy(gameObject);
        }
        Collider2D[] collidersToDamage = new Collider2D[15];
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        filter.layerMask = Owner.GetComponent<StateMachine>().hitboxMask;
        filter.useLayerMask = true;
        int colliderCount = Physics2D.OverlapCollider(hitbox, filter, collidersToDamage);
        for (int i = 0; i < colliderCount; i++)
        {
            if (collidersToDamage[i].gameObject != Owner.GetComponent<Collider2D>().gameObject)
            {
                if (!collidersDamaged.Contains(collidersToDamage[i].gameObject))
                {
                    TeamComponent hitTeamComponent = collidersToDamage[i].GetComponent<TeamComponent>();
                    if (hitTeamComponent)
                    {
                        if (hitTeamComponent.teamIndex != Owner.GetComponent<TeamComponent>().teamIndex)
                        {

                            if (collidersToDamage[i].GetComponent<Entity>())
                            {
                                int damage = UnityEngine.Random.Range(4, 12);
                                Entity entity = collidersToDamage[i].GetComponent<Entity>();
                                entity.TakeDamage(damage, Owner.GetComponent<Entity>());
                                entity.OnDamageTaken?.Invoke(-damage, Owner);
                                    if (DestroyOnContact)
                                    {
                                        Destroy(gameObject);
                                    }
                            }
                            Debug.Log(collidersToDamage[i].name + " -> Enemy has taken Damage");
                            collidersDamaged.Add(collidersToDamage[i].gameObject);
                        }
                    }
                }
            }
        }
        }
    }

    public void SetOff()
    {
        On = false;
    }

    public void SetDirection(float x)
    {
        direction = (int)x;
    }

    public void SetDuration(int duration)
    {
        this.duration = duration;
    }

    public void SetOwner(BattleCharacter battleCharacter)
    {
        Owner = battleCharacter;
    }
}
