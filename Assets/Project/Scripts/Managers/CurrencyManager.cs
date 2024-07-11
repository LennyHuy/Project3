using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; } //Check the Coins and Player scripts to know more
    public static int currency = 0;
    private TextMeshProUGUI currencyText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void SetCurrencyText(TextMeshProUGUI text) //Very useful for referencing remoted object without the need of that script (check GameManger for more details) 
    {
        currencyText = text;
        UpdateCurrencyText();
    }

    void Start()
    {
        // Load currency from saved data, if any
        UpdateCurrencyText();
        currency = PlayerPrefs.GetInt("currency", currency);
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
        UpdateCurrencyText();
        // Save currency to player preferences
        PlayerPrefs.SetInt("currency", currency);
        PlayerPrefs.Save();
    }
    public bool CanSubtractCurrency(int amount)
    {
        return currency >= amount; // Return true if only currency is equal or greater than amount
    }

    public void SubtractCurrency(int amount)
    {
        if (currency >= amount)
        {
            currency -= amount;
            UpdateCurrencyText();
            // Save currency to player preferences
            PlayerPrefs.SetInt("currency", currency);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("Not enough currency to subtract");
        }
    }

    public void UpdateCurrencyText()
    {
        if (currencyText != null)
        {
            currencyText.text = currency.ToString();
        }
    }
}
