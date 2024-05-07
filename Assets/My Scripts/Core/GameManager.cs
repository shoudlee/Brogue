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
        }
    }

}
