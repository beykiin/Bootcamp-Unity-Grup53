using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkill : MonoBehaviour
{
    public GameObject shieldPrefab;
    private GameObject currentShield;
    private bool isActive = false;
    public float shieldCooldown = 15f;
    public float shieldDuration = 7f;
    private float shieldTimer = 0f;
    private bool shieldActive = false;
    private bool canActivate = true; 

    void Start()
    {
        
        shieldActive = false;
        canActivate = true;
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
            shieldActive = false;
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

            shieldActive = true;
            Invoke("DeactivateShield", shieldDuration);
        }

        isActive = !isActive;
        shieldTimer = Time.time; 
        canActivate = false; 
    }

    void DeactivateShield()
    {
        if (currentShield != null)
        {
            Destroy(currentShield);
        }
        shieldActive = false;
    }

    public bool IsShieldActive()
    {
        return shieldActive;
    }
}
