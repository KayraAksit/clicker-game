using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public int coins = 0;  // The player's total coins
    public TextMeshProUGUI coinText;  // UI text to display the coin count

    void Start()
    {
        UpdateCoinText();  // Initialize the UI display
    }

    // Add coins and update UI
    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinText();
    }

    // Update the UI text
    public void UpdateCoinText()
    {

        coinText.text = "Coins: " + coins.ToString();
    }
}
