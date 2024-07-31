using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public Transform player;
    private Animator animator;
    private Vector3 lastPosition;
    private float moveThreshold = 0.01f;

    private void Start()
    {
        animator= GetComponent<Animator>();
        lastPosition = transform.position;
    }

    private void Update()
    {
        Vector3 currentPosition = transform.position;

        float distanceMoved = Vector3.Distance(lastPosition, currentPosition);

        if (distanceMoved > moveThreshold)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        lastPosition= currentPosition;

        RotateTowardsPlayer();
    }
    private void RotateTowardsPlayer()
    {

        if (player != null) 
        {
            Vector3 direction = (player.position - gameObject.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        
    }
}
