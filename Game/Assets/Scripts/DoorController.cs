using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject keyObject; // Key tagına sahip game object'i buraya atayın
    public float targetY = 8.45f; // Kapının ulaşacağı y ekseni
    public float moveSpeed = 2f; // Kapının hareket hızı

    private bool isMoving = false; // Kapının hareket edip etmediğini kontrol eder

    void Update()
    {
        // E tuşuna basıldığında ve karakter kapının yakınındayken kapıyı hareket ettir
        if (Input.GetKeyDown(KeyCode.E) && !isMoving)
        {
            // Kapının hareket etmeye başlamasını ve Key object'in saklanmasını başlat
            StartCoroutine(MoveDoor());
            keyObject.SetActive(false);
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
