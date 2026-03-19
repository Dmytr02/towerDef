using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private EnemyData enemyData;
    private float currentHealth;

    public void Initialize(EnemyData data)
    {
        enemyData = data;
        currentHealth = data.maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
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
