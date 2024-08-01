using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherCharchterJump : MonoBehaviour
{
    private Animator animator; 
    public float jumpInterval = 2f; 

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        
        StartCoroutine(JumpRoutine());
    }

    private IEnumerator JumpRoutine()
    {
        while (true)
        {
           
            animator.SetTrigger("Jump");

            
            yield return new WaitForSeconds(jumpInterval);
        }
    }
}
