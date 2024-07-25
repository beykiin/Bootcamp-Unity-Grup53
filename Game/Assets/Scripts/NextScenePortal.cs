using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScenePortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Karakter portalı geçti");
            int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextLevelIndex);
        }
    }
}
