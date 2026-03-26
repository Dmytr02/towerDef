using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [SerializeField] private Transform spawnPoint;

    private void Awake()
    {
        Instance = this;
    }
    
    public void SpawnEnemy(EnemyData data)
    {
        GameObject enemyObj = Instantiate(data.prefab, spawnPoint.position, Quaternion.identity);

        EnemyHealth health = enemyObj.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.Initialize(data);
        }
    }
}
