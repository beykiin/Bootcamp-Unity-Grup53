using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidController : MonoBehaviour
{
    public GameObject lid;

    public GameObject effectPrefab;
    public Transform effectSpawnPoint;
    public float effectDuration = 1.0f;

    public float openSpeed = 1.0f;

    
    private bool isOpen = false;

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E) && isOpen)
        {
            OpenLid();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            isOpen = true;
        }
    }

    private void OpenLid()
    {

        Transform lidTransform = lid.transform;


        lidTransform.localRotation = Quaternion.Euler(-50, lidTransform.localRotation.y, lidTransform.localRotation.z);
        lidTransform.localPosition = new Vector3(lidTransform.localPosition.x, 2.2f, lidTransform.localPosition.z);

        if (effectPrefab != null)
        {

            Vector3 spawnPosition = effectSpawnPoint ? effectSpawnPoint.position : transform.position;


            GameObject effectInstance = Instantiate(effectPrefab, spawnPosition, Quaternion.identity);


            Destroy(effectInstance, effectDuration);
        }
    }
}
