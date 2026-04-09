using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WaypointManager : MonoBehaviour
{

    public bool isMoving;
    public float rotationSpeed;

    public int waypointIndex;
    
    [SerializeField] private EnemyData enemyData;

    
    private void Start()
    {
        transform.position = SceneGenerator.Path[0];
        
        StartMoving();

        
    }

    public void StartMoving()
    {
        waypointIndex = 0;
        isMoving = true;
    }

    private void Update()
    {
        transform.localScale = SceneGenerator.enemyScale;
        if (!isMoving)
        {
            return;
        }

        if (waypointIndex < SceneGenerator.Path.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, SceneGenerator.Path[waypointIndex], Time.deltaTime * enemyData.moveSpeed * transform.localScale.x);
        
            var direction = transform.position - SceneGenerator.Path[waypointIndex];
            var targetRotation = Quaternion.LookRotation(-direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                
            var distance = Vector3.Distance(transform.position, SceneGenerator.Path[waypointIndex]);
            if (distance <= 0.0007f)
            {
                waypointIndex++;
            }   
            Debug.Log($"{distance}, {waypointIndex}");
        }
    }
}
