using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogFollow : MonoBehaviour
{
    public Transform target; 
    public float followSpeed = 3.0f; 
    public float stoppingDistance = 2.0f; 
    private Animator animator; 
    private Vector3 previousPosition; 

    
    private const string IsRunning = "IsRunning";

    void Start()
    {
        
        animator = GetComponent<Animator>();
        previousPosition = transform.position;
    }

    [HideInInspector] public bool shouldFollow = false; // Takip edip etmeyeceğini kontrol eder

    void Update()
    {
<<<<<<< HEAD
        float distance = Vector3.Distance(transform.position, target.position);
        Vector3 currentPosition = transform.position;

       
        float distanceMoved = Vector3.Distance(previousPosition, currentPosition);
        if (distanceMoved > 0.1f) 
        {
            animator.SetBool(IsRunning, true);
        }
        else
        {
            animator.SetBool(IsRunning, false);
        }

        
        previousPosition = currentPosition;

        
        if (distance > stoppingDistance)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * followSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * followSpeed);
=======
        if (shouldFollow)
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
>>>>>>> 48749a1446e0b70eea9177017202249ad5c72c9c
        }
    }
}
