using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform target;

    public int highObstacleLayer;
    // Start is called before the first frame update
    void Start()
    {
        highObstacleLayer = LayerMask.NameToLayer("High Obstacle");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float distance = Vector3.Distance(target.position, transform.position);
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance))
        {
            // Debug.Log("111");
            if (hit.collider.gameObject.layer == highObstacleLayer)
            {
                Debug.Log("true");
            }
            else
            {
                Debug.Log(hit.collider.gameObject.name);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, target.position);
    }
}
