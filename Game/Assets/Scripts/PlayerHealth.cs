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

    private void Start()
    {
        
        başlangıçKonumu = transform.position;
        ResetHealth();
        animator= GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
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
