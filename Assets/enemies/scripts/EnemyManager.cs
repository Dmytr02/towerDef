using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    public List<GameObject> enemies;
    private void Awake()
    {
        Instance = this;
    }

    public void SpawnEnemy(EnemyData data)
    {
        if (SceneGenerator.Path == null || SceneGenerator.Path.Count == 0)
        {
            return;
        }

        Vector3 spawnPos = SceneGenerator.Path[0];
        GameObject enemyObj = Instantiate(data.prefab, spawnPos, Quaternion.identity, SceneGenerator.m_transform);
        enemies.Add(enemyObj);
        
        EnemyHealth health = enemyObj.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.Initialize(data);
            health.OnDeath += () =>
            {
                Debug.Log("remowe enemy");
                enemies.Remove(enemyObj);
            };
        }

        WaypointManager waypoint = enemyObj.GetComponent<WaypointManager>();
        if (waypoint != null)
            waypoint.Initialize(data);
    }

    private float GetMaxHealth(EnemyData data)
    {
        return data.maxHealth * (1 + (((int)WaveManager.Instance.)));
    }
}