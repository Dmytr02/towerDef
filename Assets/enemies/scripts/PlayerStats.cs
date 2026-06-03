using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    [SerializeField] private int startCoins = 5;
    [SerializeField] private UnityEvent Death;
    private int _coins;
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip TakeDamageClip, LoseClip;

    public int coins
    {
        get { return _coins; }
        set
        {
            _coins = value; 
            coinsText.text =$"{_coins.ToString()}";
        }
    }
    public int lives = 3;

    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("Click Attack Settings")]
    [SerializeField] private float clickDamage = 5f;
    [SerializeField] private float clickCooldown = .7f;
    private float nextClickTime = 0f;
    private Camera mainCamera;

    void Awake()
    {
        Instance = this;
        coins = startCoins;
        UpdateUI();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        Death.AddListener(() =>
        {
            audioSource.PlayOneShot(LoseClip);
            print("death listener invoke");
            if (WaveManager.Instance.totalWavesCount + 1 > PlayerPrefs.GetInt("best_wave", 0))
            {
                PlayerPrefs.SetInt("best_wave", WaveManager.Instance.totalWavesCount+1);
                waveText.text = $"New best wave: {WaveManager.Instance.totalWavesCount+1}";
            }else waveText.text = $"Best wave: {PlayerPrefs.GetInt("best_wave",0)}\nCurrent wave: {WaveManager.Instance.totalWavesCount+1}";
            
        });
    }

    void Update() {
        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= nextClickTime) {
            HandleEnemyClick();
        }
    }

    private void HandleEnemyClick() {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();

            if (enemy != null) {
                enemy.TakeDamage(clickDamage);
                Debug.Log("hitted " + hit.collider.name);

            }
        }
        nextClickTime = Time.time + clickCooldown;
    }

    public void AddCoins(int amount) {
        coins += amount;
    }

    public void AddLives()
    {
        lives++;
        UpdateUI();
    }

    public void RemoveLife()
    {
        if (lives <= 0) return;
        audioSource.PlayOneShot(TakeDamageClip);
        lives--;
        if (lives <= 0) Death?.Invoke();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (lifeText != null)
        {
            lifeText.text = $"{lives}";
        }
    }
}
