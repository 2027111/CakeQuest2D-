using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : BattleObjects
{
    Vector3 initialPosition;
    Vector3 targetPosition = Vector3.zero;
    float timer;
    float timeToHit = .8f;


    private void Start()
    {
        initialPosition = transform.position;
        timeToHit = Random.Range(.2f, .4f);
    }



    public override void SetTarget(BattleCharacter _target)
    {
        base.SetTarget(_target);
        targetPosition = _target.transform.position + Vector3.up;
        Debug.Log(_target.name);
    }


    private void Update()
    {
        if(timer < timeToHit)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(initialPosition, targetPosition, timer/ timeToHit);
        }
        else
        {
            TriggerHit();
            Destroy(gameObject);
        }
    }


}
