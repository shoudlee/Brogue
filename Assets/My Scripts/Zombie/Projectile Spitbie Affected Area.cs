using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// y at -0.53 at the beginning, -0.1 at max spreading area
public class ProjectileSpitbieAffectedArea: MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float spreadingSpeed;

    private float maxY = -0.1f;
    void Start()
    {
        StartCoroutine(CoroDestoryAfterDuration());
    }

    private void Update()
    {
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
}
