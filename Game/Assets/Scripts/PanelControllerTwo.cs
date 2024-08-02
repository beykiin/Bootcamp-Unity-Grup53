using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelControllerTwo : MonoBehaviour
{
    public GameObject panel; // UI Panel

    void Start()
    {
        panel.SetActive(false); // Paneli başlangıçta kapalı yap
    }

    void OnTriggerEnter(Collider other)
    {
        // Eğer tetikleyiciye giren nesne "Player" tag'ine sahipse
        if (other.CompareTag("Player"))
        {
            panel.SetActive(true); // Paneli aç
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Eğer tetikleyiciden çıkan nesne "Player" tag'ine sahipse
        if (other.CompareTag("Player"))
        {
            panel.SetActive(false); // Paneli kapat
        }
    }
}
