using UnityEngine;

namespace Combat
{
    public class Actor : ScriptableObject
    {
        public string actorName;
        public Stats stats;
        public Sprite icon;

        public void TakeDamage(int damage)
        {
            damage -= stats.defense;
            stats.health = Mathf.Max(stats.health - Mathf.Min(damage, 0), 0);
        }

        public void Heal(int amount)
        {
            stats.health += amount;
        }
        
        public void Attack(Actor target)
        {
            target.TakeDamage(stats.attack);
        }
        
        public bool IsAlive => stats.health > 0;
    }
}