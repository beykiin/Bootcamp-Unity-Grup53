using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogFollow : MonoBehaviour
{
    public Transform target; // Takip edilecek karakter
    public float followSpeed = 3.0f; // Takip hızı
    public float stoppingDistance = 2.0f; // Karaktere olan minimum mesafe

    void Update()
    {
        // Karakter ile köpek arasındaki mesafeyi hesapla
        float distance = Vector3.Distance(transform.position, target.position);

        // Eğer köpek karaktere yeterince yakın değilse, karaktere doğru hareket et
        if (distance > stoppingDistance)
        {
            // Karaktere doğru yönel
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * followSpeed * Time.deltaTime;

            // Karaktere doğru dön
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * followSpeed);
        }
    }
}

