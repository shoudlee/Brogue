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
    [SerializeField]private ABLoader aBloader;
    [SerializeField]private UIManager uiManager;
    [SerializeField]private SFXManager sfxManager;
    [SerializeField] int gameTimeLV1;
    
    [HideInInspector]public int highObstacleLayerMask;
    [HideInInspector]public int mainFloorLayerMask;
    [HideInInspector]public int playerLayerMask;
    [HideInInspector]public int playerBulletLayerMask;
    [HideInInspector]public PlayerMovement playerMovement;
    [HideInInspector]public float currentGameTime;

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
        currentGameTime = gameTimeLV1;
    }

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>().GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (currentGameTime > 0)
        {
            currentGameTime -= Time.deltaTime;
        }

        if (currentGameTime < 0)
        {
            currentGameTime = 0;
        }
        
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
    
    public void DamageNumberEventHandler(int damge, Vector3 pos)
    {
        uiManager.DamageNumberEventHandler(damge, pos);
    }
}
