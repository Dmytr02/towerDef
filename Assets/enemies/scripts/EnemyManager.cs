using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

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
        GameObject enemyObj = Instantiate(data.prefab, spawnPos, Quaternion.identity);

        EnemyHealth health = enemyObj.GetComponent<EnemyHealth>();
        if (health != null)
            health.Initialize(data);

        WaypointManager waypoint = enemyObj.GetComponent<WaypointManager>();
        if (waypoint != null)
            waypoint.Initialize(data);
    }
}