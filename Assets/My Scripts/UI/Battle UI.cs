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

    private void Start()
    {
        UpdatePlayerPortrait();
        GameManager.Instance.playerMovement.GetHitEvent += UpdatePlayerPortrait;
    }

    private void UpdatePlayerPortrait()
    {
        float radio = GameManager.Instance.GetPlayerHealthRadio();
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
