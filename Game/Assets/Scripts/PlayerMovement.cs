using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 100f;
    public float jumpForce = 5f;  // Zıplama kuvveti

    private Rigidbody rb;
    private Animator animator;
    private float moveInput;
    private float turnInput;
    private bool isGrounded;
    public Transform characterModel;
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireballSpeed = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

    }



    void Update()
    {
        // W ve S ile ileri-geri hareket
        moveInput = Input.GetAxis("Vertical");

        // A ve D ile dönme
        turnInput = Input.GetAxis("Horizontal");

        if(moveInput != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }


        if (Input.GetKeyDown(KeyCode.Z))
        {
            Fire();
        }



        // Space tuşu ile zıplama
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }


    void Fire()
    {

        if (animator != null)
        {
            animator.SetTrigger("isTrigger");
        }


        if (fireballPrefab == null || firePoint == null)
        {
            return;
        }

        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = fireball.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = firePoint.forward * fireballSpeed;
        }


        Destroy(fireball, 1f);
        //else
        //{

        //}


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
        animator.SetBool("isJumping", true);
    }

    // Karakterin yere temas edip etmediğini kontrol etmek için
    void OnCollisionStay(Collision collision)
    {
        // Yere temas ediyorsa isGrounded true
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
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

