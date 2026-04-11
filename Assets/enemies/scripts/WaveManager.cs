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

        foreach (var entry in wave.enemies)
        {
            for (int i = 0; i < entry.count; i++)
            {
                SpawnEnemy(entry.enemyData);
                yield return new WaitForSeconds(entry.spawnInterval);
            }
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

    private void SpawnEnemy(EnemyData data)
    {
        int wave = Instance.currentWaveIndex;
        enemiesAlive++;
        
        EnemyManager.Instance.SpawnEnemy(data);
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
