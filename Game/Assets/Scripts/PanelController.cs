using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public GameObject panel; // UI Panel
    public Button actionButton; // UI Button

    void Start()
    {
        panel.SetActive(false); // Paneli başlangıçta kapalı yap
        actionButton.gameObject.SetActive(false); // Butonu başlangıçta kapalı yap
    }

    void OnTriggerEnter(Collider other)
    {
        // Eğer tetikleyiciye giren nesne "Player" tag'ine sahipse
        if (other.CompareTag("Player"))
        {
            panel.SetActive(true); // Paneli aç
            actionButton.gameObject.SetActive(true); // Butonu aç
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