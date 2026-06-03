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

    [SerializeField] private int _enemyCount = 1;

    public int enemyCount
    {
        get => _enemyCount * (5 + WaveManager.Instance.totalWavesCount * 4);
    } [SerializeField] private float _spawnInterval = 1f;

    public float spawnInterval
    {
        get => _spawnInterval * Mathf.Max(0.3f, 1.0f - WaveManager.Instance.totalWavesCount * 0.03f);
    }
}