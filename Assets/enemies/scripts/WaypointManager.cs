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
    public float speedMultiplier = 1f;
    [SerializeField] private EnemyData enemyData;
    
    private Coroutine moveCoroutine;
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

    public void ApplySlow(float factor, float duration) {
        if(moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine=StartCoroutine(SlowCoroutine(factor, duration));
    }

    private IEnumerator SlowCoroutine(float factor, float duration) {
        speedMultiplier = 1f - factor;
        yield return new WaitForSeconds(duration);
        speedMultiplier = 1f;
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
                Time.deltaTime * enemyData.moveSpeed * transform.lossyScale.x * speedMultiplier
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
        else
        {
            PlayerStats.Instance.RemoveLife();
            
            WaveManager.Instance.OnEnemyDied();

            Destroy(gameObject);
        }
    }
}
