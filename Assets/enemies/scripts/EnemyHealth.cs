using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyHealth : MonoBehaviour
{ 
    private float currentHealth;

    [SerializeField] private EnemyData enemyData;

    [SerializeField] private EnemyHPbar enemyHPbar;

    private void Awake()
    {
        if (enemyData != null)
        {
            Initialize(enemyData); // for testing
        }
    }

    public void Initialize(EnemyData data)
    {
        enemyData = data;
        currentHealth = data.maxHealth;
        
        if (enemyHPbar != null)
        {
            enemyHPbar.UpdateHPbar(enemyData.maxHealth, currentHealth);
        }
    }

    private void Update()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float amount)
    {
        if (enemyData == null) return;
        
        currentHealth -= amount;
        Debug.Log(gameObject.name + " dostal obrazenia HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            enemyHPbar.UpdateHPbar(enemyData.maxHealth, currentHealth);
        }
    }

    private void Die()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.AddCoins(enemyData.coinReward);
            
            if (enemyData.giveLife)
            {
                PlayerStats.Instance.AddLives();
            }
        }

        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.OnEnemyDied();
        }

        Destroy(gameObject);
    }
}
