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

    public void SetInput(Vector2 direction)
    {
        // Normalize the direction vector to ensure it has a magnitude of 1
        direction.Normalize();

        // Round the x and y components to the nearest integer to get one of the 8 directions
        float roundedX = Mathf.Round(direction.x);
        float roundedY = Mathf.Round(direction.y);

        // Combine the rounded components to form the final direction vector
        movementInput = new Vector2(roundedX, roundedY);

        // If both x and y are zero (which means no direction), keep it as zero
        if (movementInput.magnitude > 1)
        {
            movementInput.Normalize(); // this handles cases like (1, 1) turning into (1.41, 1.41) before normalization
        }
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

        rb2D.MovePosition(rb2D.position + (movementInput.normalized * moveSpeed * runFactor * Time.deltaTime));
    }

    public void Run(bool running)
    {
        runFactor = running ? 1.8f : 1;
    }
}
