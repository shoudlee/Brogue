using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberManager : MonoBehaviour
{
    [SerializeField] private DamageNumberPrefabUI damageNumberPrefabUI;

    public DamageNumberPrefabUI DamgeNumberGenerate(int damge, Vector3 pos)
    {
        var damgeNumberUI = Instantiate(damageNumberPrefabUI, pos, Quaternion.identity);
        damgeNumberUI.Init(damge);
        return damgeNumberUI;
    }
}
