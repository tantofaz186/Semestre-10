using System.Collections.Generic;
    using UnityEngine;

    public class SaveCoins : MonoBehaviour, ISaveable<CoinData>
    {
        public Coin prefab;
        
        public CoinData Sincronize()
        {
            CoinData data = new CoinData();
            data.coinPositions = new List<Vector3>();
            foreach (Coin coin in FindObjectsByType<Coin>(FindObjectsSortMode.None))
            {
                data.coinPositions.Add(coin.transform.position);
            }
            return data;
        }
        
        public void Load(CoinData data)
        {
            foreach (Coin coin in FindObjectsByType<Coin>(FindObjectsSortMode.None))
            {
                Destroy(coin.gameObject);
            }
            foreach (Vector3 position in data.coinPositions)
            {
                Instantiate(prefab, position, prefab.transform.rotation);
            }
        }
        
        SaveData ISaveable.Sincronize() => Sincronize();
        void ISaveable.Load(SaveData data) => Load((CoinData)data);
    }