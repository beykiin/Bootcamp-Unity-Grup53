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

    private bool isMoving = false; // Kapının hareket edip etmediğini kontrol eder
    private bool hasKey = false; // Anahtarın karakterde olup olmadığını kontrol eder

    private GameObject portalObject; // Portal tagine sahip game object

    void Start()
    {
        // "Portal" tagine sahip objeyi bul ve gizle
        portalObject = GameObject.FindWithTag("Portal");
        if (portalObject != null)
        {
            portalObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Portal tagine sahip obje bulunamadı!");
        }
    }

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
                
                // Portal objesini görünür yap
                if (portalObject != null)
                {
                    portalObject.SetActive(true);
                }
            }
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
        Debug.Log("Kapı hareketi tamamlandı");
    }
}
