using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    // Oyun ba�latma metodunu da buraya ekleyebilirsiniz, gerekirse

    // Bu metod, Quit butonuna t�kland���nda �a�r�lacak
    public void QuitGame()
    {
        // Unity Editor'da �al���rken uygulamay� durdurur
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Derlenmi� oyunda uygulamay� kapat�r
        Application.Quit();
#endif
    }
}
