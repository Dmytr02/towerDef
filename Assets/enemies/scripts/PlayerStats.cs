using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public int coins;
    public int lives = 3;

    [SerializeField] private TextMeshProUGUI lifeText;

    void Awake()
    {
        Instance = this;
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
