using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 1f; 
    public int attackDamage = 10; 
    public float attackRate = 1f; 
    private float nextAttackTime = 0f;

    private Transform player;
    private Animator animator;
    private PlayerHealth playerHealth;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform; 
        animator = GetComponent<Animator>(); 
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("SwingSword"); 
        }

        if (playerHealth == null)
        {
            playerHealth = player.GetComponent<PlayerHealth>(); 
        }

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage); 
        }
    }
}
