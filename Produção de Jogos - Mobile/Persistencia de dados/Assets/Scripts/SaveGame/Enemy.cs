using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ISaveable<EnemyInfo>
{
    public int xp, str, agi, vit, armor, level;
    public float speed;
    public string enemyName;
    public EnemyInfo enemyData;
    public Vector3 nextPatrolPosition;
    public List<Vector3> patrolPositions;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (patrolPositions.Count == 0) return;
        if (Vector3.Distance(transform.position, nextPatrolPosition) > 0.1f)
        {
            Vector3 direction = (nextPatrolPosition - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            int currentIndex = patrolPositions.IndexOf(nextPatrolPosition);
            int nextIndex = (currentIndex + 1) % patrolPositions.Count;
            nextPatrolPosition = patrolPositions[nextIndex];
        }
    }

    public EnemyInfo Sincronize()
    {
        enemyData = new EnemyInfo();
        enemyData.xp = xp;
        enemyData.str = str;
        enemyData.agi = agi;
        enemyData.vit = vit;
        enemyData.armor = armor;
        enemyData.level = level;
        enemyData.speed = speed;
        enemyData.position = transform.position;
        enemyData.enemyName = enemyName;
        enemyData.nextPatrolPosition = nextPatrolPosition;
        enemyData.patrolPositions = patrolPositions;
        Debug.Log("Player data synchronized.");
        return enemyData;
    }

    public void Load(EnemyInfo data)
    {
        enemyData = new EnemyInfo();
        enemyData = data;
        xp = enemyData.xp;
        str = enemyData.str;
        agi = enemyData.agi;
        vit = enemyData.vit;
        armor = enemyData.armor;
        level = enemyData.level;
        speed = enemyData.speed;
        transform.position = enemyData.position;
        enemyName = enemyData.enemyName;
        nextPatrolPosition = enemyData.nextPatrolPosition;
        patrolPositions = enemyData.patrolPositions;
        Debug.Log("Player data loaded.");
    }

    SaveData ISaveable.Sincronize() => Sincronize();
    void ISaveable.Load(SaveData data) => Load((EnemyInfo)data);
}