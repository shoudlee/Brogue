using System;
using System.Collections;
using System.Collections.Generic;
using Brogue.Core;
using Brogue.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public ABLoader aBloader;
    public UIManager uiManager;
    public int highObstacleLayerMask;
    public int mainFloorLayerMask;
    public int playerLayerMask;
    public int playerBulletLayerMask;

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

}
