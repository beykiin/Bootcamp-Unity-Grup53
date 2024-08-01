using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFence : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        // Eğer tetikleyiciye giren nesne "Player" tag'ine sahipse
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject); // Bu objeyi yok et
        }
    }
}