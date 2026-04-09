using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Tower Defense/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("basic information")]
    public string enemyName = "Enemy";
    public GameObject prefab;

    [Header("stats")]
    public float maxHealth = 100f;
    public float moveSpeed = 2f;
    public float damage = 10;

    [Header("rewards")]
    public int coinReward = 10;
    public bool giveLife = false;

    [Header("boss")]
    public bool isBoss = false;
}
