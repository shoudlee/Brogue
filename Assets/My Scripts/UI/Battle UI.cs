using System;
using System.Collections;
using System.Collections.Generic;
using Brogue.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private RectTransform healthMask;
    [SerializeField] private Image normalPlayerPortrait;
    [SerializeField] private Image deadPlayerPortrait;
    [SerializeField] private TextMeshProUGUI currentGameTime;
    [SerializeField] DamageNumberManager damageNumberManager;

    private void Start()
    {
        UpdatePlayerPortrait();
        GameManager.Instance.playerMovement.GetHitEvent += UpdatePlayerPortrait;

        RefreshCurrentGameTime();
        InvokeRepeating("RefreshCurrentGameTime", 1f, 1f);
    }

    private void RefreshCurrentGameTime()
    {
        currentGameTime.text = Math.Ceiling(GameManager.Instance.currentGameTime).ToString();
        if (GameManager.Instance.currentGameTime == 0)
        {
            CancelInvoke("RefreshCurrentGameTime");
        }
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

    public void DamageNumberEventHandler(int damge, Vector3 pos)
    {
        var damageNumberPrefabUI = damageNumberManager.DamgeNumberGenerate(damge, pos);
        damageNumberPrefabUI.transform.SetParent(this.transform);
    }
    
}
