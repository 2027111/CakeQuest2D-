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
        patrolling.SetLightSource();
        if (IsPlayerInSight() && patrolling.isUsable() && patrolling.OnCatchPlayer.GetPersistentEventCount() > 0 && !FadeScreen.fading)
        {
            patrolling.OnCatchPlayer?.Invoke();
        }
        else if (Vector2.Distance(character.transform.position, currentTarget.position) > patrolling.minimumPatrollingdistance)
        {
            Vector2 direction = CalculateDirection(character.transform.position, currentTarget.position);
            movement.SetInput(direction);

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

    private Vector2 CalculateDirection(Vector3 characterPosition, Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - characterPosition).normalized;

        // Prioritize X-axis over Y-axis
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            direction.y = 0; // Zero out Y component to prioritize horizontal movement
        }
        else
        {
            direction.x = 0; // Zero out X component to prioritize vertical movement
        }

        // Ensure direction is exactly one of the four cardinal directions
        if (direction.x != 0)
        {
            direction.x = Mathf.Sign(direction.x);
        }
        if (direction.y != 0)
        {
            direction.y = Mathf.Sign(direction.y);
        }

        return direction;
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
                float angleToPlayer = Vector2.Angle(movement.lookDirection, directionToPlayer);
                if (angleToPlayer <= patrolling.visionAngle / 2)
                {
                    // Raycast to detect obstacles between the patrolling character and the player
                    RaycastHit2D hit = Physics2D.Raycast(position, directionToPlayer.normalized, distanceToPlayer, patrolling.detectionLayerMask);

                    // Check if the raycast hits the player and not an obstacle
                    if (hit.collider == null)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }


}
