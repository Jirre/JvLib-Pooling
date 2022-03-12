using UnityEngine;
using JvLib.Services;

namespace JvLib.Pooling.Objects
{
    [ServiceInterface]
    public class ObjectPoolServiceManager : APoolService<ObjectPool, ObjectPoolContext, ObjectPoolGroup>
    {
        internal static readonly Vector3 PASSIVE_POSITION = new Vector3(1000f, 1000f, 1000f);

        protected override ObjectPool InitializePool(IPoolContext pContext)
        {
            ObjectPool pool = new GameObject(pContext.Id).AddComponent<ObjectPool>();
            pool.Initialize((ObjectPoolContext) pContext);
            return pool;
        }
    }

    [System.Serializable]
    public struct ObjectPoolContext : IPoolContext
    {
        [SerializeField] private string _Id;
        public string Id => _Id;
        [SerializeField] private PooledObject _Source;
        public GameObject Source => _Source.gameObject;
        [SerializeField] private int _Buffer;
        public int Buffer => _Buffer;

        [SerializeField] private string _GroupId;
        public string GroupId => _GroupId;
        [SerializeField] private float _GroupWeight;
        public float GroupWeight => _GroupWeight;
    }
}
