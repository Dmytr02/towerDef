using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    public int coinReward = 10;
    public int livesReward = 1;

    public bool isBoss = false;

    private void Start()
    {
        currentHealth = maxHealth;
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
            PlayerStats.Instance.AddCoins(coinReward);
            
            if (isBoss)
            {
                PlayerStats.Instance.AddLives();
            }
        } 
        Destroy(gameObject);
    }
}
