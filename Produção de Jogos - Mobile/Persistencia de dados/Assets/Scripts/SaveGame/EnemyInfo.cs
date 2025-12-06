using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyInfo : SaveData
{
    public int xp, str, agi, vit, armor, level;
    public float speed;
    public Vector3 position;
    public Vector3 nextPatrolPosition;
    public List<Vector3> patrolPositions;
    public string enemyName;
}
