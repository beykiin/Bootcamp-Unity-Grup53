using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject keyObject; // Key tagına sahip game object
    public Transform player; // Karakterin transform'u
    public float targetY = 8.45f; // Kapının ulaşacağı y ekseni
    public float moveSpeed = 2f; // Kapının hareket hızı
    public float interactionDistance = 2f; // Kapıya ne kadar yaklaşıldığında etkileşime geçilebileceği mesafe
    public DogFollow dogFollow; // Köpeğin takip script'i

    private bool isMoving = false; // Kapının hareket edip etmediğini kontrol eder
    private bool hasKey = false; // Anahtarın karakterde olup olmadığını kontrol eder

    void Update()
    {
        // Anahtar karakterde mi kontrol et
        hasKey = Vector3.Distance(keyObject.transform.position, player.position) < interactionDistance;

        // E tuşuna basıldığında, karakter kapının yakınındayken ve anahtar yanındayken kapıyı hareket ettir
        if (Input.GetKeyDown(KeyCode.E) && !isMoving && Vector3.Distance(transform.position, player.position) < interactionDistance && hasKey)
        {
            StartCoroutine(MoveDoor());
            keyObject.SetActive(false);
            dogFollow.shouldFollow = true; // Köpeğin takip etmeye başlaması
        }
    }

    IEnumerator MoveDoor()
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, targetY, startPosition.z);

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }
}
