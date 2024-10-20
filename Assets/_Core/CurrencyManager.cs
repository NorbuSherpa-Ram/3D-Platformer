using System;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField]
    private int coinCount;

    public EventHandler OnCoinUpdate;

    private void Start()
    {
        Coin.OnAnyCoinCollect += Coin_OnAnyCoinCollect;
        coinCount = PlayerPrefs.GetInt("Coin", 0);
        UpdateCoinCount();
    }

    private void OnDestroy()
    {
        Coin.OnAnyCoinCollect -= Coin_OnAnyCoinCollect;
    }

    private void Coin_OnAnyCoinCollect(object sender, EventArgs e)
    {
        UpdateCoinCount();
    }

    private void UpdateCoinCount()
    {
        coinCount++;
        OnCoinUpdate?.Invoke(this, EventArgs.Empty);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Coin", coinCount);
    }

    public int GetCoinCount() => coinCount;
}
