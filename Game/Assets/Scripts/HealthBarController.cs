using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Image healthBarFill;

    public void SetHealth(int currentHealth, int maxHealth)
    {
        healthBarFill.fillAmount = (float)currentHealth / maxHealth;
    }
}
