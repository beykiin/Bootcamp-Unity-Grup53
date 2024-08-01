using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngerButtonController : MonoBehaviour
{
    public Button myButton; // UI Button
    public float activeAlpha = 1f; // Buton aktifken alfa değeri
    public float inactiveAlpha = 0.5f; // Buton pasifken alfa değeri
    private bool canPress = true; // Butona basılabilir mi?
    private float cooldownTime = 10f; // Bekleme süresi

    private Image buttonImage;

    void Start()
    {
        buttonImage = myButton.GetComponent<Image>(); // Butonun Image bileşenine erişim
        SetButtonAlpha(inactiveAlpha); // Butonu başlangıçta pasif yap
        myButton.interactable = false; // Butonu başlangıçta etkileşime kapat
    }

    void Update()
    {
        // "Z" tuşuna basıldığında ve buton basılabilir durumda olduğunda
        if (Input.GetKeyDown(KeyCode.Z) && canPress)
        {
            OnButtonPress();
        }
    }

    void OnButtonPress()
    {
        // Butona basma işlemi
        Debug.Log("Button Pressed");

        // Butonu pasif yap ve rengini soluk yap
        canPress = false;
        SetButtonAlpha(inactiveAlpha);
        myButton.interactable = false;

        // 10 saniye sonra butonu yeniden aktif hale getir
        Invoke("ResetButton", cooldownTime);
    }

    void ResetButton()
    {
        // Butonu yeniden aktif yap
        canPress = true;
        SetButtonAlpha(activeAlpha);
        myButton.interactable = true;
    }

    void SetButtonAlpha(float alpha)
    {
        // Butonun alfa değerini değiştir
        Color color = buttonImage.color;
        color.a = alpha;
        buttonImage.color = color;
    }
}