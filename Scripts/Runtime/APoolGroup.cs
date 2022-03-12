using System.Collections.Generic;
using UnityEngine;

namespace JvLib.Pooling
{
    public abstract class APoolGroup<T>
    {
        private List<PoolContext> _pools;
        private float _totalWeight;

        protected int Count => _pools.Count;

        internal void Add(T pValue, float pWeight)
        {
            _pools ??= new List<PoolContext>();
            _pools.Add(new PoolContext(pValue, pWeight));
            _totalWeight += pWeight;
        }

        public T GetRandomPool()
        {
            if (_pools == null || _pools.Count <= 0)
                return default;

            float random = Random.Range(0, _totalWeight);
            foreach (PoolContext c in _pools)
                if ((random -= c.Weight) <= 0)
                    return c.Pool;

            return default;
        }

        protected IEnumerator<T> GetEnumerator()
        {
            foreach (PoolContext c in _pools)
            {
                if (c.Pool == null) continue;
                yield return c.Pool;
            }
        }

        private struct PoolContext
        {
            public T Pool { get; }
            public float Weight { get; }

            public PoolContext(T pPool, float pWeight)
            {
                Pool = pPool;
                Weight = pWeight;
            }
        }
    }
}
