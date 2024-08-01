using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 100f;
    public float jumpForce = 5f;
    public float angerSkillDuration = 2f;
    public float attackDamage = 50f;
    public Collider swordCollider;
    public TextMeshProUGUI angerSkillCooldownText;
    public Button angerSkillCooldownButton;
    public Color cooldownBackgroundColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);

    private Rigidbody rb;
    private Animator animator;
    private float moveInput;
    private float turnInput;
    private bool isGrounded;
    public Transform characterModel;
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireballSpeed = 10f;
    private bool isAngerSkillActive = false;

    public float angerSkillCooldown = 10f;
    private float lastAngerSkillTime = -10f;
    private float skillStartTime;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (swordCollider != null)
        {
            swordCollider.enabled = false; 
        }
        if (angerSkillCooldownText != null) //*
        { //*
            angerSkillCooldownText.text = ""; //*
            SetButtonBackgroundColor(angerSkillCooldownButton, Color.clear);
        } //*
    }



    void Update()
    {
        
        moveInput = Input.GetAxis("Vertical");

        
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



        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        UpdateAngerSkillCooldown();
    }


    void Fire()
    {

        if (Time.time - lastAngerSkillTime < angerSkillCooldown)
        {
            return; 
        }

        lastAngerSkillTime = Time.time;

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
        StartCoroutine(ActivateAngerSkill());



    }

    IEnumerator ActivateAngerSkill()
    {
        isAngerSkillActive = true;
        skillStartTime = Time.time;
        if (swordCollider != null)
        {
            swordCollider.enabled = true; 
        }

        yield return new WaitForSeconds(angerSkillDuration);

        isAngerSkillActive = false;

        if (swordCollider != null)
        {
            swordCollider.enabled = false; 
        }
    }

    void UpdateAngerSkillCooldown()
    {
        float remainingCooldown = Mathf.Max(0, angerSkillCooldown - (Time.time - lastAngerSkillTime));

        if (angerSkillCooldownText != null)
        {
            if (remainingCooldown > 0)
            {
                angerSkillCooldownText.text = Mathf.RoundToInt(remainingCooldown).ToString() + "s";
                if (angerSkillCooldownButton != null)
                {
                    SetButtonBackgroundColor(angerSkillCooldownButton, cooldownBackgroundColor); // Gri arka plan
                }
            }
            else
            {
                angerSkillCooldownText.text = "";
                if (angerSkillCooldownButton != null)
                {
                    SetButtonBackgroundColor(angerSkillCooldownButton, Color.clear); // Arka planı şeffaf yap
                }
            }
        }
    }

    void FixedUpdate()
    {
        
        MoveCharacter(moveInput);

        
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

    
    void OnCollisionStay(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isAngerSkillActive && other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }
    void SetButtonBackgroundColor(Button button, Color color)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = color;
        }
    }
}

