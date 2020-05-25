using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Initialize speed parameters
    public float forwardSpeed;
    public float sideSpeed;

    // Input parameters
    private float horizontalInput;
    private float forwardInput;

    // Rigid body variable
    private Rigidbody playerRb;
    private float pushDirection;
    public float pushPower = 1000f;
    private float dashVelocity = 10f;
    public float jumpForce = 10f;

    private Vector3 moveVect = Vector3.zero;

    // Animator
    private Animator playerAnim;
    private bool onGround;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        onGround = true;
    }

    void Update()
    {

        // Movement inputs
        moveVect = Vector3.zero;
        forwardInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        if (forwardInput > 0)
        {
            playerAnim.SetFloat("Speed_f", 1);
        } else
        {
            playerAnim.SetFloat("Speed_f", 0);
        }

        // Using square root to transform input for a snappier accelaration
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
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerAnim.SetBool("Jump_b", true);
            onGround = false;
        }

        /* Dash code
        // When player is adding sideways movement
        if (horizontalInput != 0)
        {
            if (horizontalInput > 0)
            {
                pushDirection = Mathf.Ceil(horizontalInput);
            }
            else
            {
                pushDirection = Mathf.Floor(horizontalInput);
            }
            playerRb.AddForce(Vector3.right * dashVelocity * pushDirection, ForceMode.Impulse);
        }
        // Otherwise, dash forward
        else
        {
            playerRb.AddForce(Vector3.forward * dashVelocity, ForceMode.VelocityChange);
        }
        */

    }

    private void FixedUpdate()
    {
        // Move player given inputs
        playerRb.MovePosition(transform.position + moveVect * Time.fixedDeltaTime);
    }

    // Pushes objects player contacts
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Competitor"))
        {
            Rigidbody competitorRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            competitorRigidbody.AddForce(awayFromPlayer * pushPower, ForceMode.Impulse);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            playerAnim.SetBool("Jump_b", false);
        }
    }
}
