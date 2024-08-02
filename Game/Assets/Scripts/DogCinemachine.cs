using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DogCinemachine : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineCamera; // Cinemachine sanal kameranız
    public Camera normalCamera; // Normal oyun kameranız
    public Collider dogCollider; // Köpeğin Collider bileşeni

    private bool isCameraSwitched = false;

    void Start()
    {
        // Oyuna başladığında Cinemachine kamera devre dışı bırakılır
        if (cinemachineCamera != null)
        {
            cinemachineCamera.gameObject.SetActive(false);
            Debug.Log("Cinemachine Camera disabled.");
        }

        // Normal kamerayı etkinleştir
        if (normalCamera != null)
        {
            normalCamera.gameObject.SetActive(true);
            Debug.Log("Normal Camera enabled.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == dogCollider)
        {
            Debug.Log("Dog collided!");
            SwitchToCinemachine();
        }
    }

    void SwitchToCinemachine()
    {
        if (!isCameraSwitched)
        {
            Debug.Log("Switching to Cinemachine Camera");

            // Cinemachine kamerayı etkinleştir
            if (cinemachineCamera != null)
            {
                cinemachineCamera.gameObject.SetActive(true);
                Debug.Log("Cinemachine Camera Enabled");
            }

            // Normal kamerayı devre dışı bırak
            if (normalCamera != null)
            {
                normalCamera.gameObject.SetActive(false);
                Debug.Log("Normal Camera Disabled");
            }

            isCameraSwitched = true;

            // Cinemachine kameranın süresi dolduğunda geri dönüş işlevini başlat
            StartCoroutine(WaitAndSwitchBack(5f)); // 5 saniye örnek bir süre, ihtiyacınıza göre ayarlayın
        }
    }

    IEnumerator WaitAndSwitchBack(float waitTime)
    {
        Debug.Log("Waiting to switch back");
        yield return new WaitForSeconds(waitTime);

        // Normal kameraya geri dön
        if (cinemachineCamera != null)
        {
            cinemachineCamera.gameObject.SetActive(false);
            Debug.Log("Cinemachine Camera Disabled");
        }

        if (normalCamera != null)
        {
            normalCamera.gameObject.SetActive(true);
            Debug.Log("Normal Camera Enabled");
        }

        isCameraSwitched = false;
    }

}
