using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumberPrefabUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private float upSpeed;
    [SerializeField] private float lifeTime;

    // called before update
    public void Init(int damge)
    {
        damageText.text = $" -{damge}";
        StartCoroutine(CoroDestroySelf(lifeTime));
    }
    private void FixedUpdate()
    {
        transform.position += new Vector3(0, upSpeed, 0);
    }


    private IEnumerator CoroDestroySelf(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
