using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 100f;
    public float jumpForce = 5f;  // Zıplama kuvveti

    private Rigidbody rb;
    private float moveInput;
    private float turnInput;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // W ve S ile ileri-geri hareket
        moveInput = Input.GetAxis("Vertical");

        // A ve D ile dönme
        turnInput = Input.GetAxis("Horizontal");

        // Space tuşu ile zıplama
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Hareket
        MoveCharacter(moveInput);

        // Dönme
        TurnCharacter(turnInput);
    }

    void MoveCharacter(float input)
    {
        Vector3 move = transform.forward * input * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    void TurnCharacter(float input)
    {
        float turn = input * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    void Jump()
    {
        Vector3 jump = new Vector3(0f, jumpForce, 0f);
        rb.AddForce(jump, ForceMode.Impulse);
    }

    // Karakterin yere temas edip etmediğini kontrol etmek için
    void OnCollisionStay(Collision collision)
    {
        // Yere temas ediyorsa isGrounded true
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Yerden ayrıldığında isGrounded false
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}

