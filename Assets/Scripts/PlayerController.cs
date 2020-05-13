using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Initialize speed parameters
    public float forwardSpeed;
    public float sideSpeed;
    public float dashDistance;

    // Input parameters
    private float horizontalInput;
    private float forwardInput;
    // Rigid body variable
    private Rigidbody playerRb;
    private float push;
    private float dash;

    private Vector3 moveVect = Vector3.zero;
    public Vector3 dashVelocity;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();   
    }

    void Update()
    {

        // Movement inputs
        moveVect = Vector3.zero;
        forwardInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput > 0)
        {
            moveVect.x = Mathf.Sqrt(horizontalInput) * sideSpeed;
        } else if (horizontalInput < 0)
        {
            moveVect.x = Mathf.Sqrt(horizontalInput * -1) * -sideSpeed;
        }

        if (forwardInput > 0)
        {
            moveVect.z = Mathf.Sqrt(forwardInput) * forwardSpeed;
        } else if (forwardInput < 0)
        {
            moveVect.z = Mathf.Sqrt(forwardInput * -1) * -forwardSpeed;
        }

   
        // On pressing space, player dashes horiztonally, or forward to push
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // When player is adding sideways movement
            if (horizontalInput != 0)
            {
                if (horizontalInput > 0)
                {
                    push = Mathf.Ceil(horizontalInput);
                } else
                {
                    push = Mathf.Floor(horizontalInput);
                }
                playerRb.AddForce(Vector3.right * 10 * push, ForceMode.VelocityChange);
            }
            // Otherwise, dash forward
            else
            {
                playerRb.AddForce(Vector3.forward * 10, ForceMode.VelocityChange);
            }
        }
       
    }
    private void FixedUpdate()
    {
        // Move player given inputs
        playerRb.MovePosition(transform.position + moveVect * Time.fixedDeltaTime);
    }
}
