using System;
using System.Collections;
using System.Collections.Generic;
using Brogue.Player;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private RectTransform healthMask;
    [SerializeField] private Image normalPlayerPortrait;
    [SerializeField] private Image deadPlayerPortrait;
    private PlayerMovement playerMovement;
    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.Log("Player script not found. ");
        }
    }

    void Update()
    {
        UpdatePlayerPortrait();
    }

    private float GetPlayerHealthRadio()
    {
        
        return (float)playerMovement.currentHp/playerMovement.playerMaxHp;
    }
    private void UpdatePlayerPortrait()
    {
        float radio = GetPlayerHealthRadio();
        if (radio > 0)
        {
            healthMask.sizeDelta = new Vector2(healthMask.sizeDelta.x, (1-radio) * 100);
        }
        else if (normalPlayerPortrait.IsActive())
        {
            normalPlayerPortrait.gameObject.SetActive(false);
            deadPlayerPortrait.gameObject.SetActive(true);
        }
    }
}
