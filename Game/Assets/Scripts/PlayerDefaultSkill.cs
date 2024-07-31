using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefaultSkill : MonoBehaviour
{
    public float attackDamage = 5f; 
    public Collider swordCollider; 
    public string attackAnimationTrigger = "Attack"; 

    private Animator animator; 
    private HashSet<Collider> enemiesInRange = new HashSet<Collider>(); 

    void Start()
    {
        animator = GetComponent<Animator>();

        if (swordCollider != null)
        {
            swordCollider.enabled = false; 
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            UseDefaultSkill();
        }
    }

    void UseDefaultSkill()
    {
        if (animator != null)
        {
            animator.SetTrigger(attackAnimationTrigger); 
        }

        if (swordCollider != null)
        {
            swordCollider.enabled = true; 
        }

        ApplyDamageToEnemies();
        if (swordCollider != null)
        {
            StartCoroutine(DisableSwordCollider());
        }
    }

    IEnumerator DisableSwordCollider()
    {
        yield return null; 
        if (swordCollider != null)
        {
            swordCollider.enabled = false;
        }
    }

    void ApplyDamageToEnemies()
    {
        foreach (Collider enemyCollider in enemiesInRange)
        {
            if (enemyCollider != null)
            {
                EnemyHealth enemyHealth = enemyCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage); 
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other); 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other); 
        }
    }

}
