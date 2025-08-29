using UnityEngine;

namespace PowerUp
{
    [RequireComponent(typeof(Collider))]
    public abstract class PowerUp : MonoBehaviour, ICollectable
    {
        public abstract void OnCollect();

        protected void OnTriggerEnter(Collider other)
        {
            OnCollect();
        }

        private void OnValidate()
        {
            if (gameObject.layer != LayerMask.NameToLayer("PowerUp"))
            {
                Debug.LogError("PowerUp must have the layer 'PowerUp'");
            }
        }
    }
}