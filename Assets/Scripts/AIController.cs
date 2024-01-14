using System.Collections;
using UnityEngine;

public class AIController : Controller
{
    public float detectionRadius = 10f;
    public float retreatHealthThreshold = 75f;
    public float minDistanceFromTarget = 3f;

    private BattleCharacter battleCharacter;
    private Transform target;
    private AttackPlacement lastAttackPlacement; // Keep track of the last attack performed

    private void Start()
    {
        battleCharacter = GetComponent<BattleCharacter>();
        SetController();
        StartCoroutine(FindTargetRoutine());
        lastAttackPlacement = AttackPlacement.NONE; // Initialize to NONE
    }

    private void Update()
    {
        if (canControl)
        {
            if (target != null)
            {
                    MoveTowardsTarget();
               
            }
            else if (battleCharacter.entity.characterObject.Health <= retreatHealthThreshold)
            {
                // If no valid target is found and health is below the retreat threshold, retreat
                Retreat();
            }
        }
        else
        {
    
                    OnAttackRelease?.Invoke();
                    wasdInput = Vector2.zero;
        }


        OnMovement?.Invoke(wasdInput);
    }
    private IEnumerator FindTargetRoutine()
    {

        yield return new WaitForSeconds(.5f); // Adjust the interval as needed

        GameObject nearestEnemy = FindNearestEnemy();
        target = nearestEnemy.transform;

    }
    private void Retreat()
    {
        if (target != null)
        {
            // Calculate the direction vector away from the target
            Vector2 retreatDirection = (transform.position - target.position).normalized;

            wasdInput = retreatDirection;
            // Move away from the target

            // Check if the AI is far enough from the target
            if (Vector2.Distance(transform.position, target.position) >= minDistanceFromTarget)
            {
                // Reset movement input

                wasdInput = Vector2.zero;
            }
        }
    }

    private void MoveTowardsTarget()
    {
        if (target != null)
        {
            // Calculate the direction vector towards the target
            Vector2 directionToTarget = target.position - transform.position;

            
                // Move towards the target
                wasdInput = directionToTarget.normalized;
            

            // Update the AI's movement input

            // Check if the AI is close enough to attack
            if (Vector2.Distance(transform.position, target.position) <= 2)
            {
                AttackRoutine();
            }
            else
            {
                // Stop the attack if not in range
                OnAttackRelease?.Invoke();
            }
        }
    }

    private GameObject FindNearestEnemy()
    {
        BattleCharacter[] bcs = FindObjectsOfType<BattleCharacter>();

        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (BattleCharacter bc in bcs)
        {
            if (bc.gameObject != gameObject)
            {
                TeamComponent enemyTeamComponent = bc.GetComponent<TeamComponent>();

                if (bc != null && enemyTeamComponent != null && enemyTeamComponent.teamIndex != battleCharacter.GetComponent<TeamComponent>().teamIndex && !battleCharacter.entity.characterObject.isDead)
                {
                    float distance = Vector2.Distance(transform.position, bc.transform.position);

                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestEnemy = bc.gameObject;
                    }
                }
            }
        }

        return nearestEnemy;
    }



    private AttackPlacement FindMatchingAttack()
    {
        // Iterate through the available attacks in your AttackListManager
        foreach (AttackData attackData in battleCharacter.GetComponent<AttackListManager>().AttackList)
        {
            // For simplicity, assume that the attack should have a hitbox (change this condition based on your actual requirements)
            if (attackData.hitboxes != null && attackData.hitboxes.Count > 0)
            {
                // Check if the enemy is within the hitbox range
                if (IsEnemyInHitboxRange(attackData.hitboxes[0]))
                {
                    return attackData.attackPlacement;
                }
            }
        }

        // Return NONE if no matching attack is found
        return AttackPlacement.NONE;
    }




    private void AttackRoutine()
    {
        // Find an attack that connects with the enemy
        AttackPlacement attackPlacement = FindMatchingAttack();

        // Check if the current wasdInput matches the required input for the attack
        if (IsMatchingAttackInput(attackPlacement))
        {
            // Check if the selected attack is different from the last one
            if (attackPlacement != lastAttackPlacement)
            {
                // Perform the attack
                OnAttackPressed?.Invoke();
                lastAttackPlacement = attackPlacement; // Update the last attack performed
            }
            else
            {
                // Choose a different attack if the same one is selected again
                ChooseDifferentAttack();
            }
        }
        else
        {
            // Stop the attack if the input does not match
            OnAttackRelease?.Invoke();
        }
    }

    private void ChooseDifferentAttack()
    {
        // Iterate through the available attacks and choose a different one
        foreach (AttackData attackData in battleCharacter.GetComponent<AttackListManager>().AttackList)
        {
            if (attackData.hitboxes != null && attackData.hitboxes.Count > 0)
            {
                AttackPlacement attackPlacement = attackData.attackPlacement;
                if (attackPlacement != lastAttackPlacement)
                {
                    // Perform the attack
                    OnAttackPressed?.Invoke();
                    lastAttackPlacement = attackPlacement; // Update the last attack performed
                    return;
                }
            }
        }

        // If no different attack is found, stop the attack
        OnAttackRelease?.Invoke();
    }

    private bool IsMatchingAttackInput(AttackPlacement attackPlacement)
    {
        float x = 0f;
        float y = 0f;

        switch (attackPlacement)
        {
            case AttackPlacement.SLIGHT:
                // Example: Slight attack requires moving left or right
                x = (wasdInput.x < 0) ? -1f : 1f;
                break;
            case AttackPlacement.DLIGHT:
                // Example: Dlight attack requires moving down
                y = -1f;
                break;
            case AttackPlacement.NLIGHT:
                // Example: Nlight attack requires no specific movement
                break;
        }

        // Set the modified values for wasdInput
        wasdInput = new Vector2(x, y);

        // You can also adjust the threshold or conditions based on your game's requirements
        return true;
    }
    private bool IsEnemyInHitboxRange(HitBoxInfo hitbox)
    {
        Debug.Log("POPOPO");
        // Check if there is a valid target
        if (target != null)
        {
            // Calculate the position of the hitbox relative to the character's position
            Vector2 hitboxPosition = (Vector2)transform.position + hitbox.offset;

            // Calculate the boundaries of the hitbox
            float leftBoundary = hitboxPosition.x - hitbox.size.x / 2;
            float rightBoundary = hitboxPosition.x + hitbox.size.x / 2;
            float bottomBoundary = hitboxPosition.y - hitbox.size.y / 2;
            float topBoundary = hitboxPosition.y + hitbox.size.y / 2;

            // Check if the target's position is within the hitbox boundaries
            if (target.position.x >= leftBoundary && target.position.x <= rightBoundary &&
                target.position.y >= bottomBoundary && target.position.y <= topBoundary)
            {
                return true;
            }
        }

        // Return false if there is no valid target or the target is not within the hitbox boundaries
        return false;
    }

}
