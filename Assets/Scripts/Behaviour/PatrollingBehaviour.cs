using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingBehaviour: CharacterBehaviour
{

    Transform currentTarget;
    Patrolling patrolling;
    
    public PatrollingBehaviour() : base()
    {
    }



    public override void OnEnter(Character player)
    {
        base.OnEnter(player);
        patrolling = player.GetComponent<Patrolling>();
        if (!patrolling)
        {
            player.ChangeState(new NothingBehaviour());
        }


        currentTarget = patrolling.GetCurrentWayPoint();

    }



    public override void Handle()
    {
        if(Vector2.Distance(character.transform.position, currentTarget.position) > patrolling.minimumPatrollingdistance)
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
            currentTarget = patrolling.NextWayPoint();
        }
    }


    public override void OnExit()
    {
        movement.movementInput = Vector2.zero;
    }







}
