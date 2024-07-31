using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic backgrouMusic;

    void Awake()
    {
        if(backgrouMusic == null)
        {
            backgrouMusic = this;
            DontDestroyOnLoad(backgrouMusic);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
