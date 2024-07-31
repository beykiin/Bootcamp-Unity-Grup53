using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSkill : MonoBehaviour
{
    public int healAmount = 20;
    

    public GameObject healEffectPrefab;

    private bool canHeal = true; 
    private float healCooldown = 10f; 
    private float healTimer = 0f;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (canHeal)
            {
                Heal(20);
                healTimer = Time.time;
                canHeal = false; 
            }
        }

        if (!canHeal && Time.time - healTimer >= healCooldown)
        {
            canHeal = true;
        }
    }

    void Heal(int amount)
    {
        if (playerHealth != null)
        {
            int previousHealth = playerHealth.currentHealth;
            playerHealth.Heal(healAmount);
            int newHealth = playerHealth.currentHealth;

            if (healEffectPrefab != null)
            {
                GameObject effect = Instantiate(healEffectPrefab, transform.position, Quaternion.identity);
                effect.transform.parent = transform;
                effect.transform.localPosition = Vector3.zero;
                Destroy(effect, 3f);
            }
            Debug.Log($"Previous Health: {previousHealth}, New Health: {newHealth}");
        }
    }
}
