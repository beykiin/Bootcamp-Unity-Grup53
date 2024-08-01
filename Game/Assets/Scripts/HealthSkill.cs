using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthSkill : MonoBehaviour
{
    public int healAmount = 20;
    

    public GameObject healEffectPrefab;

    private bool canHeal = true; 
    private float healCooldown = 10f; 
    private float healTimer = 0f;
    private PlayerHealth playerHealth;


    public Button healSkillButton;
    public TextMeshProUGUI healCooldownText;
    public Color cooldownColor = Color.grey;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        UpdateButtonVisuals();
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
                UpdateButtonVisuals();
            }
        }

        if (!canHeal && Time.time - healTimer >= healCooldown)
        {
            canHeal = true;
            UpdateButtonVisuals();
        }
        if (!canHeal)
        {
            UpdateCooldownTimer();
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

    void UpdateCooldownTimer()
    {
        float remainingCooldown = Mathf.Max(0, healCooldown - (Time.time - healTimer));

        if (healCooldownText != null)
        {
            if (remainingCooldown > 0)
            {
                healCooldownText.text = Mathf.RoundToInt(remainingCooldown).ToString() + "s";
            }
            else
            {
                healCooldownText.text = "";
            }
        }
    }

    void UpdateButtonVisuals()
    {
        if (healSkillButton != null)
        {
            Image buttonImage = healSkillButton.GetComponent<Image>();

            if (buttonImage != null)
            {
                if (canHeal)
                {
                    buttonImage.color = Color.clear;
                    if (healCooldownText != null)
                    {
                        healCooldownText.text = "";
                    }
                }
                else
                {
                    buttonImage.color = cooldownColor;
                }
            }
        }
    }
}
