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
    [SerializeField] private float speedScalePerWave = 0.05f;
    [SerializeField] private float damageScalePerWave = 0.10f;

    private int currentWaveIndex = 0;
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
        if (waveInProgress || currentWaveIndex >= waves.Count)
        {
            return;
        }

        startWaveButton.interactable = false;
        StartCoroutine((RunWave(waves[currentWaveIndex])));
    }

    private IEnumerator RunWave(WaveData wave)
    {
        waveInProgress = true;
        yield return new WaitForSeconds(wave.delayBeforeWave);

        for (int i = 0; i < wave.enemyCount; i++)
        {
            EnemyData baseData = GetRandomEnemy(wave);
            if (baseData == null)
            {
                continue;
            }

            EnemyData scaledData = GetScaledEnemyData(baseData, currentWaveIndex);

            enemiesAlive++;

            EnemyManager.Instance.SpawnEnemy(scaledData);

            yield return new WaitForSeconds(wave.spawnInterval);
        }

        yield return new WaitUntil(() => enemiesAlive <= 0);

        waveInProgress = false;
        currentWaveIndex++;

        UpdateUI();

        if (currentWaveIndex < waves.Count)
        {
            startWaveButton.interactable = true;
        }
    }

    private EnemyData GetRandomEnemy(WaveData wave)
    {
        if (wave.enemyPool == null || wave.enemyPool.Count == 0) return null;

        float totalWeight = 0f;
        foreach (var entry in wave.enemyPool)
            totalWeight += entry.spawnWeight;

        float roll = UnityEngine.Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var entry in wave.enemyPool)
        {
            cumulative += entry.spawnWeight;
            if (roll <= cumulative)
                return entry.enemyData;
        }

        return wave.enemyPool[0].enemyData;
    }

    private EnemyData GetScaledEnemyData(EnemyData original, int waveIndex)
    {
        EnemyData scaled = ScriptableObject.CreateInstance<EnemyData>();

        scaled.enemyName = original.enemyName;
        scaled.prefab = original.prefab;
        scaled.coinReward = original.coinReward;
        scaled.giveLife = original.giveLife;
        scaled.isBoss = original.isBoss;

        float multiplier = 1f + waveIndex;

        scaled.maxHealth = original.maxHealth * (1f + healthScalePerWave * waveIndex);
        scaled.moveSpeed = original.moveSpeed * (1f + speedScalePerWave * waveIndex);
        scaled.damage = original.damage * (1f + damageScalePerWave * waveIndex);

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
            waveInfoText.text = $"wave {currentWaveIndex + 1} / {waves.Count}";
        }
    }
}
