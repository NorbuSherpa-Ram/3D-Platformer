using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField]
    private int coinCount;



    private void Start()
    {
        Coin.OnAnyCoinCollect += Coin_OnAnyCoinCollect;
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
    }

}
