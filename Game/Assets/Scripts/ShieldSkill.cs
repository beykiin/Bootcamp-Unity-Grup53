using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShieldSkill : MonoBehaviour
{
    public GameObject shieldPrefab;
    private GameObject currentShield;
    private bool isActive = false;
    public float shieldCooldown = 10f;
    public float shieldDuration = 5f;
    private float shieldTimer = 0f;
    private bool shieldActive = false;
    private bool canActivate = true;

    public Button shieldSkillButton; 
    public TextMeshProUGUI shieldCooldownText; 
    public Color cooldownColor = Color.grey;

    void Start()
    {
        
        
        canActivate = true;
        UpdateButtonVisuals();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (canActivate)
            {
                ToggleShield();
            }
        }
        else if (!canActivate && Time.time - shieldTimer >= shieldCooldown)
        {
            canActivate = true;
            UpdateButtonVisuals();
        }
        if (!canActivate)
        {
            UpdateCooldownTimer();
        }
    }

    void ToggleShield()
    {
        if (isActive)
        {
            
            if (currentShield != null)
            {
                Destroy(currentShield);
            }
            isActive = false;
            CancelInvoke("DeactivateShield");
        }
        else
        {
            
            currentShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
            currentShield.transform.parent = transform;
            currentShield.transform.localPosition = Vector3.zero;

            Renderer renderer = currentShield.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = true;
            }

            isActive = true;
            Invoke("DeactivateShield", shieldDuration);
        }

        
        shieldTimer = Time.time; 
        canActivate = false;
        UpdateButtonVisuals();
    }

    void DeactivateShield()
    {
        if (currentShield != null)
        {
            Destroy(currentShield);
        }
        isActive = false;
        UpdateButtonVisuals();
    }


    void UpdateCooldownTimer()
    {
        float remainingCooldown = Mathf.Max(0, shieldCooldown - (Time.time - shieldTimer));

        if (shieldCooldownText != null)
        {
            if (remainingCooldown > 0)
            {
                shieldCooldownText.text = Mathf.RoundToInt(remainingCooldown).ToString() + "s";
            }
            else
            {
                shieldCooldownText.text = "";
            }
        }

       
    }


    void UpdateButtonVisuals()
    {
        if (shieldSkillButton != null)
        {
            Image buttonImage = shieldSkillButton.GetComponent<Image>();

            if (buttonImage != null)
            {
                if (canActivate)
                {
                    buttonImage.color = Color.clear;
                    if (shieldCooldownText != null)
                    {
                        shieldCooldownText.text = ""; 
                    }
                }
                else
                {
                    buttonImage.color = cooldownColor; 
                }
            }
        }
    }

    public bool IsShieldActive()
    {
        return isActive;
    }
}
