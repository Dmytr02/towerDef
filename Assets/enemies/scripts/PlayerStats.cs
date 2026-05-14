using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    [SerializeField] private int startCoins = 5;
    
    private int _coins;

    public int coins
    {
        get { return _coins; }
        set
        {
            _coins = value; 
            coinsText.text =$"coins {_coins.ToString()}";
        }
    }
    public int lives = 3;

    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private TextMeshProUGUI coinsText;

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

                nextClickTime = Time.time + clickCooldown;
            }
        }
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
        lives--;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (lifeText != null)
        {
            lifeText.text = $"lives {lives}";
        }
    }
}
