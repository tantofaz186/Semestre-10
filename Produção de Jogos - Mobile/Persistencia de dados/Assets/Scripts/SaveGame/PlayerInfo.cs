using System;
using UnityEngine;

[System.Serializable]
public class PlayerInfo : SaveData
{
    public int xp, str, agi, vit, armor, level;
    public float speed;
    public Vector3 position;
    public string playerName;
}
