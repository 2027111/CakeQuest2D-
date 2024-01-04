using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{


    Animator anim;
    Movement pm;
    bool isMoving = false;
    [SerializeField] float animationDeadZone = 0;

    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<Movement>();
        anim = GetComponent<Animator>();
        pm.LookAtEvent += LookAt;
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = pm.GetInput().magnitude > animationDeadZone;
        anim.SetBool("IsMoving", isMoving);
        if (isMoving)
        {
            anim.SetFloat("HorizontalMov", pm.GetInput().x);
            anim.SetFloat("VerticalMov", pm.GetInput().y);
        }
    }


    public void LookAt(Vector2 direction)
    {

        anim.SetFloat("HorizontalMov", direction.x);
        anim.SetFloat("VerticalMov", direction.y);
    }

}
