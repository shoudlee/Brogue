using System;
using System.Collections;
using System.Collections.Generic;
using Brogue.Player;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

// y at -0.53 at the beginning, -0.1 at max spreading area
public class ProjectileSpitbieAffectedArea: MonoBehaviour
{
    [Header("Battle Parameters")]
    [SerializeField] private float duration;
    [SerializeField] private float spreadingSpeed;
    [SerializeField] private int damageLevel;
    [SerializeField] private float deceleration;
    private PlayerMovement player;

    private float maxY = -0.1f;

    private void Awake()
    {
        player = null;
        StartCoroutine(CoroDestoryAfterDuration());
    }
    
    
    private void FixedUpdate()
    {
        if (transform.position.y <= maxY)
        {
            float offset = spreadingSpeed * 1;
            Vector3 offsetedPosition = new Vector3(transform.position.x, transform.position.y + spreadingSpeed,
                transform.position.z);
            transform.position = offsetedPosition;
        }
        
    }

    private IEnumerator CoroDestoryAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == GameManager.Instance.playerLayerMask)
        {
            player = other.gameObject.GetComponent<PlayerMovement>();
            player.PlayerInPoison(damageLevel);
            player.movementAlter = deceleration;
            Debug.Log("player in poison!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == GameManager.Instance.playerLayerMask)
        {
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            player.PlayerOutOfPoison(damageLevel);
            player.movementAlter = 1;
            Debug.Log("player Out of poison!");
        }
    }

    private void OnDestroy()
    {
        if (player is not null)
        {
            player.PlayerOutOfPoison(damageLevel);
            player.movementAlter = 1;
        }
    }
}
