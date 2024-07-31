using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSkill : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Heal(20);
        }
    }

    void Heal(int amount)
    {
        int missingHealth = maxHealth - currentHealth;
        int healAmount = Mathf.Min(amount, missingHealth);
        currentHealth += healAmount;
        healthBar.SetHealth(currentHealth);
    }
}
