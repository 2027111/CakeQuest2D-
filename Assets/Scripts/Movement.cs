using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{



    public float moveSpeed = 5;
    public Rigidbody2D rb2D;
    public float runFactor = 1f;
    public float coneAngle = 45f;
    public float rayDistance = 1f;
    public int coneRayCount = 10;
    public LayerMask obstacleLayer; // The layer mask for obstacles
    public event Action<Vector2> LookAtEvent;
    public Vector2 movementInput = Vector2.zero;
    public Vector2 lookDirection = Vector2.zero;
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

        if (movementInput.magnitude > 0)
        {

            lookDirection = movementInput;
        }

        // If both x and y are zero (which means no direction), keep it as zero
        if (movementInput.magnitude > 1)
        {
            movementInput.Normalize(); // this handles cases like (1, 1) turning into (1.41, 1.41) before normalization

        }



    }



    public void LookAt(Vector2 direction)
    {
        SetInput(direction);
        SetInput(Vector2.zero);
        LookAtEvent?.Invoke(direction);
    }

    public void SetPosition(Vector3 newPosition)
    {
            transform.position = newPosition;
        
    }



    public void MoveCharacter()
    {
        Vector2 direction = movementInput.normalized;
        float distance = moveSpeed * runFactor * Time.deltaTime;

        //// Cast a ray in the direction of movement
        //List<RaycastHit2D> hits = new List<RaycastHit2D>(Physics2D.RaycastAll(rb2D.position, direction, distance, obstacleLayer));
        //foreach(RaycastHit2D hit in hits)
        //{
        //    // Check if the ray hit anything
        //    if (hit.collider.gameObject != gameObject)
        //    {
        //        distance = 0;
        //        Vector2 dist = (hit.collider.gameObject.transform.position - transform.position).normalized;
        //        direction -= dist;
        //        // Optionally handle what happens if an obstacle is detected
        //        Debug.Log("Obstacle detected: " + hit.collider.name);
        //        // If the ray didn't hit anything, move the character
        //    }
        //}
        rb2D.MovePosition(rb2D.position + (direction * distance));

    }

    public void Run(bool running)
    {
        runFactor = running ? 1.8f : 1;
    }
}
