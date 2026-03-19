using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public int coins;
    public int lives = 3;

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
    }
}
