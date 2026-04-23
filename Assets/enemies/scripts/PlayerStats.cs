using TMPro;
using UnityEngine;

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

    void Awake()
    {
        Instance = this;
        coins = startCoins;
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
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
