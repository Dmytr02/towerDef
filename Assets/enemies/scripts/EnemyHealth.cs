using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyHealth : MonoBehaviour
{
    private EnemyData enemyData;
    private float currentHealth;

    [SerializeField] private EnemyHPbar enemyHPbar;

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
        Debug.Log(gameObject.name + " dostał obrażenia! HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            {
                enemyHPbar.UpdateHPbar(enemyData.maxHealth, currentHealth);
            }
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

        Destroy(gameObject);
    }
}
