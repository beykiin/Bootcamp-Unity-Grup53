using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnPlayerContact : MonoBehaviour
{

    private Renderer objectRenderer;
    private Collider objectCollider;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Eğer çarpan obje "Player" tag'ine sahipse
        if (collision.gameObject.CompareTag("Player"))
        {
            // Objeyi görünmez yap ve 2 saniye sonra yeniden etkinleştir
            StartCoroutine(DestroyAndRespawn());
        }
    }

    private IEnumerator DestroyAndRespawn()
    {
        // 2 saniye bekle
        yield return new WaitForSeconds(2f);

        // Objeyi devre dışı bırak (görünmez yap)
        objectRenderer.enabled = false;
        objectCollider.enabled = false;


        // 3 saniye bekle
        yield return new WaitForSeconds(4.5f);

        // Objeyi tekrar etkinleştir (görünür yap)
        objectRenderer.enabled = true;
        objectCollider.enabled = true;
    }
}
