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

    private void Start()
    {
        
        başlangıçKonumu = transform.position;
        ResetHealth();
        animator= GetComponent<Animator>();
        shieldSkill = GetComponent<ShieldSkill>();
    }

    public void TakeDamage(int damage)
    {
        if (shieldSkill != null && shieldSkill.IsShieldActive())
        {
            damage /= 2;
            Debug.Log(currentHealth);
            
        }
        
        

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
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
