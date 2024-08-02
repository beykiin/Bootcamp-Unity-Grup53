using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private Vector3 başlangıçKonumu;
    private Animator animator;

    public GameObject birthEffectPrefab;
    private ShieldSkill shieldSkill;
    public HealthBarController healthBarController;
    private void Start()
    {
        
        başlangıçKonumu = transform.position;
        ResetHealth();
        animator= GetComponent<Animator>();
        shieldSkill = GetComponent<ShieldSkill>();
        if (healthBarController != null)
        {
            healthBarController.SetHealth(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if (shieldSkill != null && shieldSkill.IsShieldActive())
        {
            damage /= 2;
            Debug.Log(currentHealth);
            
        }
        
        

        currentHealth -= damage;

        if (healthBarController != null)
        {
            healthBarController.SetHealth(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        if (healthBarController != null)
        {
            healthBarController.SetHealth(currentHealth, maxHealth);
        }
    }

    void Die()
    {

        if (animator != null)
        {
            animator.SetTrigger("Die"); 
        }

        Invoke("ResetCharacter", 1f); 
    }

    void ResetHealth()
    {
        currentHealth = maxHealth;

        if (healthBarController != null)
        {
            healthBarController.SetHealth(currentHealth, maxHealth);
        }
    }

    void ResetCharacter()
    {
        transform.position = başlangıçKonumu;
        ResetHealth();
        StartCoroutine(ShowBirthEffect());

        if (animator != null)
        {
            animator.SetTrigger("Idle");
        }
    }
    IEnumerator ShowBirthEffect()
    {
        GameObject effect = Instantiate(birthEffectPrefab, transform.position, Quaternion.identity);
        effect.transform.parent = transform;
        yield return new WaitForSeconds(2f); 
        Destroy(effect); 
    }
}
