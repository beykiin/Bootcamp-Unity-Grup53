using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public List<GameObject> objectsToReveal; // Gizli olan ve görünür yapılacak nesnelerin listesi

    void Start()
    {
        // Oyun başladığında nesneleri gizle
        HideObjects();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box")) // Butona basacak olan nesneler
        {
            RevealObjects();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box")) // Butondan ayrılan nesneler
        {
            HideObjects();
        }
    }

    private void RevealObjects()
    {
        foreach (GameObject obj in objectsToReveal)
        {
            obj.SetActive(true);
        }
    }

    private void HideObjects()
    {
        foreach (GameObject obj in objectsToReveal)
        {
            obj.SetActive(false);
        }
    }
}
