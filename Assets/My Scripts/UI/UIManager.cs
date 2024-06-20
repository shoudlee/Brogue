using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Brogue.UI;
using UnityEngine;

namespace Brogue.UI{
public class UIManager : MonoBehaviour
{
    [SerializeField] private DefeatedUI defeatedUI;
    [SerializeField] BattleUI battleUI;
    // [SerializeField] private DamageNumberManager damageNumberManager;
    
    public void ShowDefeatedUI()
    {
        defeatedUI.gameObject.SetActive(true);
    }

    public void HideDefeatedUI()
    {
        defeatedUI.gameObject.SetActive(false);
    }
    public void DamageNumberEventHandler(int damge, Vector3 pos)
    {
        battleUI.DamageNumberEventHandler(damge, pos);
    }
    
}
}