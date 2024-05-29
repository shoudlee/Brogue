using System;
using System.Collections;
using System.Collections.Generic;
using Brogue.Core;
using Brogue.Player;
using Brogue.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private ABLoader aBloader;
    private UIManager uiManager;
    private SFXManager sfxManager;
    
    public int highObstacleLayerMask;
    public int mainFloorLayerMask;
    public int playerLayerMask;
    public int playerBulletLayerMask;
    public PlayerMovement playerMovement;

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        highObstacleLayerMask = LayerMask.NameToLayer("High Obstacle");
        mainFloorLayerMask = LayerMask.NameToLayer("Main Floor");
        playerLayerMask = LayerMask.NameToLayer("Player");
        playerBulletLayerMask = LayerMask.NameToLayer("Player Bullet");
    }

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>().GetComponent<PlayerMovement>();
    }

    public float GetPlayerHealthRadio()
    {
        return (float)playerMovement.currentHp/playerMovement.playerMaxHp;
    }

    public Mesh GetRandomZombieMesh(string zombieType)
    {
        return aBloader.GetRandomZombieMesh(zombieType);
    }

    public void ShowDefeatedUI()
    {
        uiManager.ShowDefeatedUI();
    }
}
