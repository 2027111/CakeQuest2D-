using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugDisplay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI DebugText;

    BattleCharacter CC;
    StateMachine stateMachine;
    Animator animator;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        CC = transform.parent.GetComponent<BattleCharacter>();
        stateMachine = transform.parent.GetComponent<StateMachine>();
        animator = transform.parent.GetComponentInChildren<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DebugText)
        {
            string animationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            DebugText.text = $"FSMState: {(stateMachine? stateMachine.CurrentState.GetType():"null")}  \n AttackState : {CC.attackPlacement} \n AttackAnim: {animationName} \n X.Vel: {rb.velocity.x.ToString("F2")}\n Y.Vel: {rb.velocity.y.ToString("F2")}\n";// 
        }
    }
}
