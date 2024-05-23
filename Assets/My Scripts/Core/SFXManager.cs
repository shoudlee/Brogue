using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Random = UnityEngine.Random;

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
        
        audioSource.PlayOneShot(playerGetHitSFX[Random.Range(0,playerGetHitSFX.Count)]);
    }
    
}
