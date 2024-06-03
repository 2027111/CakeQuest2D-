using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingBehaviour : CharacterBehaviour
{
    Transform currentTarget;
    Patrolling patrolling;
    Character player;

    public PatrollingBehaviour() : base()
    {
    }

    public override void OnEnter(Character character)
    {
        base.OnEnter(character);
        patrolling = character.GetComponent<Patrolling>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();

        if (patrolling)
        {
            if (patrolling.isUsable())
            {
                currentTarget = patrolling.GetCurrentWayPoint();
            }
            return;
        }
            character.ChangeState(new NothingBehaviour());
    }

    public override void Handle()
    {
        if (IsPlayerInSight())
        {
            patrolling.OnCatchPlayer?.Invoke();
        }
        else if (Vector2.Distance(character.transform.position, currentTarget.position) > patrolling.minimumPatrollingdistance)
        {
            Vector2 direction = (currentTarget.position - character.transform.position).normalized;
            movement.movementInput = direction;

            if (movement.movementInput != Vector2.zero)
            {
                movement.MoveCharacter();
            }
        }
        else
        {
            if (time < patrolling.timeBetweenPoint)
            {
                movement.movementInput = Vector2.zero;
                time += Time.deltaTime;
            }
            else
            {
                time = 0;
                currentTarget = patrolling.NextWayPoint();
            }
        }
    }

    public override void OnExit()
    {
        movement.movementInput = Vector2.zero;
    }

    private bool IsPlayerInSight()
    {
        Vector2 position = character.transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, patrolling.visionRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                Vector2 directionToPlayer = (Vector2)collider.transform.position - position;
                float distanceToPlayer = directionToPlayer.magnitude;

                // Check if the player is within the vision angle
                float angleToPlayer = Vector2.Angle(character.transform.right, directionToPlayer);
                if (angleToPlayer <= patrolling.visionAngle / 2)
                {
                    // Raycast to detect obstacles between the patrolling character and the player
                    RaycastHit2D hit = Physics2D.Raycast(position, directionToPlayer.normalized, distanceToPlayer, patrolling.detectionLayerMask);

                    // Check if the raycast hits the player and not an obstacle
                    if (hit.collider == null)
                    {
                        Debug.Log(patrolling.gameObject.name + " Player Caught");
                        return true;
                    }
                }
            }
        }

        return false;
    }


}
