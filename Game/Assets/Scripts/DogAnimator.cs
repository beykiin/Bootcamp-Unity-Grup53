using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAnimator : MonoBehaviour
{
    private Animator animator;

    
    private Vector3 previousPosition;

    
    public float positionChangeThreshold = 0.1f;

    
    private const string IsRunning = "IsRunning";

    private void Start()
    {
        
        animator = GetComponent<Animator>();

        
        previousPosition = transform.position;
    }

    private void Update()
    {
        
        Vector3 currentPosition = transform.position;

        
        float distanceMoved = Vector3.Distance(previousPosition, currentPosition);

        
        if (distanceMoved > positionChangeThreshold)
        {
            animator.SetBool(IsRunning, true);
        }
        else
        {
            
            animator.SetBool(IsRunning, false);
        }

        
        previousPosition = currentPosition;
    }
}
