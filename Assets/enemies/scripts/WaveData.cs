using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewWave", menuName = "Tower Defense/WaveData")]
public class WaveData : ScriptableObject
{
    [System.Serializable]
    public class EnemyPoolEntry
    {
        public EnemyData enemyData;
        [Range(0f, 1f)] public float spawnWeight = 1f; 
    }

    public List<EnemyPoolEntry> enemyPool;

    public int enemyCount = 10;          
    public float spawnInterval = 1f;    
}