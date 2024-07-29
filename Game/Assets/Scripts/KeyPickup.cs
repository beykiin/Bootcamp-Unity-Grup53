using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public bool hasKey = false; 
    public Transform keyPosition; // Karakterin başının üstünde anahtarı taşıyacağı pozisyon
    public GameObject key; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key") && !hasKey)
        {
            // Anahtarı alır ve başının üstünde taşır
            key = other.gameObject;
            key.transform.position = keyPosition.position;
            key.transform.parent = keyPosition;
            hasKey = true;
        }
    }

    void Update()
    {
        // Anahtarı almayı tetiklemek için E tuşuna basıldığında
        if (Input.GetKeyDown(KeyCode.E) && key != null && !hasKey)
        {
            // Anahtarı alır ve başının üstünde taşır
            key.transform.position = keyPosition.position;
            key.transform.parent = keyPosition;
            hasKey = true;
        }
    }
}

