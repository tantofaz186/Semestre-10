namespace Pooling
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Pool<T> where T : MonoBehaviour, IPoolable
    {
        private readonly Queue<T> pool = new();
        private readonly T prefab;
        private readonly Transform parent;

        public Pool(T prefab, int initialSize, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;

            for (int i = 0; i < initialSize; i++) CreateNew();
        }

        public T Get()
        {
            T obj = pool.Count > 0 ? pool.Dequeue() : CreateNew();
            obj.gameObject.SetActive(true);
            obj.OnSpawned();
            return obj;
        }

        public void ReturnToPool(T obj)
        {
            obj.OnDespawned();
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }

        private T CreateNew()
        {
            T obj = Object.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
            return obj;
        }
    }
}