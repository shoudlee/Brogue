using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    [SerializeField] List<AudioClip> playerGetHitSFX;
    [SerializeField]  AudioSource audioSource;
    void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayPlayerGetHitAudio()
    {
        
    }
    
}
