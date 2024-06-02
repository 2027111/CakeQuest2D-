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
            currentTarget = patrolling.GetCurrentWayPoint();
        }
        else
        {
            character.ChangeState(new NothingBehaviour());
        }
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
        Vector2 directionToPlayer = player.transform.position - character.transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        // Check if the player is within the vision radius
        if (distanceToPlayer <= patrolling.visionRadius)
        {
            float angleToPlayer = Vector2.Angle(movement.movementInput, directionToPlayer);

            // Check if the player is within the vision angle
            if (angleToPlayer <= patrolling.visionAngle / 2)
            {
                Debug.Log("Player is in");
                // Raycast to detect obstacles between the patrolling character and the player
                RaycastHit2D hit = Physics2D.Raycast(character.transform.position + new Vector3(movement.movementInput.x, movement.movementInput.y, 0), directionToPlayer.normalized, patrolling.visionRadius);
                if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
                {
                    Debug.Log("Player Caught");
                    return true;
                }
            }
        }

        return false;
    }


}
