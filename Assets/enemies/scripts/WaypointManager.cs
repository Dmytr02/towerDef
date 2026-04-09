using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointManager : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();

    public bool isMoving;
    public float rotationSpeed;

    public int waypointIndex;
    
    [SerializeField] private EnemyData enemyData;

    
    private void Start()
    {
        StartMoving();
    }

    public void StartMoving()
    {
        waypointIndex = 0;
        isMoving = true;
    }

    private void Update()
    {
        if (!isMoving)
        {
            return;
        }

        if (waypointIndex < waypoints.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].position, Time.deltaTime * enemyData.moveSpeed);
        
            var direction = transform.position - waypoints[waypointIndex].position;
            var targetRotation = Quaternion.LookRotation(-direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                
            var distance = Vector3.Distance(transform.position, waypoints[waypointIndex].position);
            if (distance <= 0.05f)
            {
                waypointIndex++;
            }   
        }
    }
}
