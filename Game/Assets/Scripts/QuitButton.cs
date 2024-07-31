using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    // Oyun baþlatma metodunu da buraya ekleyebilirsiniz, gerekirse

    // Bu metod, Quit butonuna týklandýðýnda çaðrýlacak
    public void QuitGame()
    {
        // Unity Editor'da çalýþýrken uygulamayý durdurur
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Derlenmiþ oyunda uygulamayý kapatýr
        Application.Quit();
#endif
    }
}
