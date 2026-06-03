using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Tower Defense/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("basic information")]
    public string enemyName {
        get => name;
        set => name = value;
    }
    public GameObject prefab;

    [Header("stats")]
    [SerializeField] private float _maxHealth = 100f;

    public float maxHealth
    {
        get => maxHealth * (1 + Mathf.Pow(WaveManager.Instance.totalWavesCount/5, 0.1f));
    }
    public float moveSpeed = 2f;

    [Header("rewards")]
    public int coinReward = 10;
    public bool giveLife = false;

    [Header("boss")]
    public bool isBoss = false;
}
