using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Brogue.UI;
using UnityEngine;

namespace Brogue.UI{
public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform defeatedUI;


    // private void Awake()
    // {
    //     if (Instance is null)
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    //     
    // }

    public void ShowDefeatedUI()
    {
        defeatedUI.gameObject.SetActive(true);
    }

    public void HideDefeatedUI()
    {
        defeatedUI.gameObject.SetActive(false);
    }
}
}