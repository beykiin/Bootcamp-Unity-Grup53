using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepDogScript : MonoBehaviour
{
    private Rigidbody rb;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        
        if (!isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, -9.81f, rb.velocity.z); 
        }
    }
}
