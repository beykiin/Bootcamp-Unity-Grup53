using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClosing : MonoBehaviour
{
    public GameObject keyObject; // Key tagına sahip game object
    public Transform player; // Karakterin transform'u
    public float targetX = -0.401f; // Kapının ulaşacağı x ekseni
    public float moveSpeed = 2f; // Kapının hareket hızı
    public float interactionDistance = 2f; // Kapıya ne kadar yaklaşıldığında etkileşime geçilebileceği mesafe
    public float positionThreshold = 0.001f; // Hedef konuma ulaşma hassasiyeti

    private bool isMoving = false; // Kapının hareket edip etmediğini kontrol eder
    private bool hasKey = false; // Anahtarın karakterde olup olmadığını kontrol eder

    void Update()
    {
        // Anahtar karakterde mi kontrol et
        hasKey = Vector3.Distance(keyObject.transform.position, player.position) < interactionDistance;
        Debug.Log("Has Key: " + hasKey);

        // E tuşuna basıldığında, karakter kapının yakınındayken ve anahtar yanındayken kapıyı hareket ettir
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E tuşuna basıldı");
            if (!isMoving && Vector3.Distance(transform.position, player.position) < interactionDistance && hasKey)
            {
                Debug.Log("Kapı hareket etmeye başladı");
                StartCoroutine(MoveDoor());
                keyObject.SetActive(false);
            }
        }
    }

    IEnumerator MoveDoor()
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(targetX, startPosition.y, startPosition.z);

        while (Vector3.Distance(transform.position, targetPosition) > positionThreshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Hedef konuma tam olarak ulaş
        transform.position = targetPosition;
        isMoving = false;
        Debug.Log("Kapı hareketi tamamlandı");
    }
}
