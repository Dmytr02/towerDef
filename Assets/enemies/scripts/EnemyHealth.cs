using System;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(AudioSource))]
public class EnemyHealth : MonoBehaviour
{ 
    private float currentHealth;

    [SerializeField] private EnemyData enemyData;

    [SerializeField] private EnemyHPbar enemyHPbar;

    [SerializeField] private WaypointManager waypointManager;
    public Action OnDeath;
    private bool isInitialized = false;
    
    public AudioSource audioSource;
    public AudioClip DeathSound, SpawnSound, TakeDamaneSound;

    private void Awake()
    {
        if (enemyData != null && !isInitialized)
        {
            Initialize(enemyData);
        }
    }

    public void Initialize(EnemyData data)
    {
        if(!audioSource) audioSource = GetComponent<AudioSource>();
        isInitialized = true;
        enemyData = data;
        currentHealth = data.maxHealth;
        audioSource.PlayOneShot(SpawnSound);
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
        audioSource.PlayOneShot(TakeDamaneSound);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            enemyHPbar.UpdateHPbar(enemyData.maxHealth, currentHealth);
        }
    }

    public void Die(bool isKilled = true)
    {
        if (isKilled)
        {
            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.AddCoins(enemyData.coinReward);
                audioSource.PlayOneShot(DeathSound);

                if (enemyData.giveLife)
                {
                    PlayerStats.Instance.AddLives();
                }
            }

            if (WaveManager.Instance != null)
            {
                WaveManager.Instance.OnEnemyDied();
            }
        }

        Debug.Log("Death Invoke");
        OnDeath.Invoke();
        Destroy(gameObject);
    }
}
