using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{



    public float moveSpeed = 5;
    public Rigidbody2D rb2D;
    public float runFactor = 1f;

    public event Action<Vector2> LookAtEvent;
    public Vector2 movementInput = Vector2.zero;

    public Vector2 GetInput()
    {
        return movementInput;
    }

    // Start is called before the first frame update
    void Start()
    {

   


        rb2D = GetComponent<Rigidbody2D>();
        
    }


    public void LookAt(Vector2 direction)
    {
        LookAtEvent?.Invoke(direction);
    }

    public void SetPosition(Vector3 newPosition)
    {
            transform.position = newPosition;
        
    }



    public void MoveCharacter()
    {

        rb2D.MovePosition(rb2D.position + (movementInput * moveSpeed * runFactor * Time.deltaTime));
    }

    public void Run(bool running)
    {
        runFactor = running ? 1.8f : 1;
    }
}
