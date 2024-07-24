using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnPlayerContact : MonoBehaviour
{ 
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
        gameObject.SetActive(false);

        // 3 saniye bekle
        yield return new WaitForSeconds(3f);

        // Objeyi tekrar etkinleştir (görünür yap)
        gameObject.SetActive(true);
    }
}
