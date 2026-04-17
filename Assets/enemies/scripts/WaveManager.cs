using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [SerializeField] private List<WaveData> waves;
    [SerializeField] private Button startWaveButton;
    [SerializeField] private TextMeshProUGUI waveInfoText;

    [SerializeField] private float healthScalePerWave = 0.15f;

    private int currentWaveIndex = 0;
    private int totalWavesCount = 0;
    private bool waveInProgress = false;
    private int enemiesAlive = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
        startWaveButton.onClick.AddListener(StartNextWave);
    }

    public void StartNextWave()
    {
        startWaveButton.interactable = false;
        StartCoroutine((RunWave(waves[currentWaveIndex])));
    }

    private IEnumerator RunWave(WaveData wave)
    {
        waveInProgress = true;

        bool bossSpawned = false;

        for (int i = 0; i < wave.enemyCount; i++)
        {
            EnemyData baseData = GetRandomEnemy(wave, bossSpawned);
            if (baseData == null)
            {
                continue;
            }

            if (baseData.isBoss)
            {
                bossSpawned = true;
            }

            EnemyData scaledData = GetScaledEnemyData(baseData, totalWavesCount);

            enemiesAlive++;

            EnemyManager.Instance.SpawnEnemy(scaledData);

            yield return new WaitForSeconds(wave.spawnInterval);
        }

        yield return new WaitUntil(() => enemiesAlive <= 0);

        waveInProgress = false;
        currentWaveIndex++;
        totalWavesCount++;

        if (currentWaveIndex >= waves.Count)
        {
            currentWaveIndex = 0;
        }

        UpdateUI();

        startWaveButton.interactable = true;

        if (PlayerStats.Instance.lives <= 0)
        {
            startWaveButton.interactable = false;

            Debug.Log("no lives left, game over");
        }
    }

    private EnemyData GetRandomEnemy(WaveData wave, bool bossAlreadySpawned)
    {
        if (wave.enemyPool == null || wave.enemyPool.Count == 0) return null;

        var pool = bossAlreadySpawned
            ? wave.enemyPool.FindAll(e => !e.enemyData.isBoss)
            : wave.enemyPool;

        if (pool.Count == 0) return null;

        float totalWeight = 0f;
        foreach (var entry in pool)
            totalWeight += entry.spawnWeight;

        float roll = UnityEngine.Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var entry in pool)
        {
            cumulative += entry.spawnWeight;
            if (roll <= cumulative)
                return entry.enemyData;
        }

        return pool[0].enemyData;
    }

    private EnemyData GetScaledEnemyData(EnemyData original, int waveIndex)
    {
        EnemyData scaled = ScriptableObject.CreateInstance<EnemyData>();

        scaled.enemyName = original.enemyName;
        scaled.prefab = original.prefab;
        scaled.coinReward = original.coinReward;
        scaled.giveLife = original.giveLife;
        scaled.isBoss = original.isBoss;

        scaled.maxHealth = original.maxHealth * (1f + healthScalePerWave * waveIndex);
        scaled.moveSpeed = original.moveSpeed;

        return scaled;
    }

    public void OnEnemyDied()
    {
        enemiesAlive--;
    }

    private void UpdateUI()
    {
        if (currentWaveIndex < waves.Count)
        {
            waveInfoText.text = $"wave {totalWavesCount + 1}";
        }
    }
}
