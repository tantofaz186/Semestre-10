
using System.Xml.Serialization;

[XmlInclude(typeof(PlayerInfo))]
[XmlInclude(typeof(EnemyInfo))]
[XmlInclude(typeof(CoinData))]
[System.Serializable]
public abstract class SaveData
{
    
}