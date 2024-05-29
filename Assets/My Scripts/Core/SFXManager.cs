using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Random = UnityEngine.Random;

public class SFXManager : MonoBehaviour
{
    private static SFXManager Instance;
    [SerializeField] List<AudioClip> playerGetHitSFX;
    [SerializeField]  AudioSource audioSource;
    void Awake()
    {
        //DontDestroyOnLoad only works for root GameObjects or components on root GameObjects.
        if (Instance is null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameManager.Instance.playerMovement.GetHitEvent += PlayPlayerGetHitAudio;
    }

    public void PlayPlayerGetHitAudio()
    {
        audioSource.PlayOneShot(playerGetHitSFX[Random.Range(0,playerGetHitSFX.Count)]);
    }
    
}
