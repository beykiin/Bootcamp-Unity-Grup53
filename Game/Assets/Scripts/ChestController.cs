using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public GameObject key;  // Anahtar nesnesi
    private bool isNearChest = false;  // Karakter sandığa yakın mı

    void Start()
    {
        // Başlangıçta anahtarı gizle
        key.SetActive(false);
    }

    void Update()
    {
        // Eğer karakter sandığa yakınsa ve 'E' tuşuna basıldıysa
        if (isNearChest && Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Karakter ekstra trigger collider'a yaklaşırsa
        if (other.CompareTag("Player"))
        {
            isNearChest = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Karakter ekstra trigger collider'dan uzaklaşırsa
        if (other.CompareTag("Player"))
        {
            isNearChest = false;
        }
    }

    private void OpenChest()
    {
        // Anahtarı görünür yap
        key.SetActive(true);
        // İstersen sandık açılma animasyonu ekleyebilirsin
        // Örnek: GetComponent<Animator>().SetTrigger("Open");
    }
}

