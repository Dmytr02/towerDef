using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewWave", menuName = "Tower Defense/WaveData")]
public class WaveData : ScriptableObject
{
   [System.Serializable]
   public class EnemySpawnEntry
   {
      public EnemyData enemyData;
      public int count;
      public float spawnInterval;
   }

   public List<EnemySpawnEntry> enemies;
   public float delayBeforeWave = 1f;
}
