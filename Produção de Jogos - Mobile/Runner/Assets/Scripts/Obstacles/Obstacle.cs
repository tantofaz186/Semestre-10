using UnityEngine;

namespace Obstacles
{
    [RequireComponent(typeof(Collider))]
    public abstract class Obstacle : MonoBehaviour
    {
        protected abstract void OnPlayerCollision();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnPlayerCollision();
            }
        }
    }
}