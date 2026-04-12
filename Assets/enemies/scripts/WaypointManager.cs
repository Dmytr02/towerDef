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
        if (enemyData != null)
        {
            StartMoving();
        }
    }

    public void Initialize(EnemyData data)
    {
        enemyData = data;
        StartMoving();
    }

    public void StartMoving()
    {
        if (SceneGenerator.Path == null || SceneGenerator.Path.Count == 0)
        {
            return;
        }

        transform.position = SceneGenerator.Path[0];
        waypointIndex = 0;
        isMoving = true;
    }
    private void Update()
    {
        if (!isMoving || enemyData == null) return;

        transform.localScale = SceneGenerator.enemyScale;

        if (waypointIndex < SceneGenerator.Path.Count)
        {
            Vector3 target = SceneGenerator.Path[waypointIndex];

            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                Time.deltaTime * enemyData.moveSpeed * transform.localScale.x
            );

            var direction = transform.position - target;
            if (direction != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(-direction, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

            if (Vector3.Distance(transform.position, target) <= 0.0007f)
            {
                waypointIndex++;
            }
        }
    }
}
