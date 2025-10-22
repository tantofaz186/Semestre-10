using System;
using Collectables;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CoinCount : MonoBehaviour
{
    [SerializeField] private uint coinsCollected = 0;
    TextMeshProUGUI coinsCollectedText;
    
    private void Awake()
    {
        coinsCollectedText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Coin.OnCoinCollected += AddCoin;
    }

    private void OnDestroy()
    {
        Coin.OnCoinCollected -= AddCoin;
    }

    public void AddCoin(Coin coin)
    {
        coinsCollected++;
        coinsCollectedText.text = coinsCollected.ToString();
    }
    
    
}
