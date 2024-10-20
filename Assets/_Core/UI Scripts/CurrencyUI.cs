using System;
using TMPro;
using UnityEngine;

/// <summary>
/// RESPONSIBLE FOR SHOWING IN  COIN COUNT IN GAME UI  
/// </summary>
public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    private CurrencyManager currencyManager;
    private void Start()
    {
        currencyManager = FindFirstObjectByType<CurrencyManager>();

        if (currencyManager != null)
            currencyManager.OnCoinUpdate += CurrencyManager_OnCoinUpdate;
    }

    //CURRENCY MANAGER EVENT
    private void CurrencyManager_OnCoinUpdate(object sender, EventArgs e)
    {
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        coinText.text = $"Coin Count : {currencyManager.GetCoinCount().ToString()}";
    }

    private void OnDestroy()
    {
        currencyManager.OnCoinUpdate -= CurrencyManager_OnCoinUpdate;
    }
}
