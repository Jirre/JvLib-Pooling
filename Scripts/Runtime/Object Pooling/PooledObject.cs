using UnityEngine;

namespace JvLib.Pooling.Objects
{
    public class PooledObject : MonoBehaviour
    {
        private ObjectPool _pool;
        public string Id => _pool.Id;

        internal void Initialize(ObjectPool pPool)
        {
            _pool = pPool;
        }

        public void Deactivate()
        {
            _pool.Deactivate(gameObject);
        }
    }
}
