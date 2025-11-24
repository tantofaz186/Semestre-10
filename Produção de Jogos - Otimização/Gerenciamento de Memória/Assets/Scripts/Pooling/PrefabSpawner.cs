namespace Pooling
{
    using UnityEngine;

    public abstract class PrefabSpawner<T> : MonoBehaviour where T : MonoBehaviour, IPoolable
    {
        [SerializeField] protected T prefab;
        [SerializeField] protected int initialSize;

        private Pool<T> pool;

        protected virtual void Awake()
        {
            pool = new Pool<T>(prefab, initialSize, transform);
        }

        public virtual T Spawn(Vector3 position, Quaternion rotation)
        {
            T obj = pool.Get();
            obj.transform.SetPositionAndRotation(position, rotation);
            return obj;
        }

        public virtual void Despawn(T obj)
        {
            pool.ReturnToPool(obj);
        }
    }
}