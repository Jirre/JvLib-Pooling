using JvLib.Services;
using UnityEngine;

namespace JvLib.Pooling.Particles
{
    [ServiceInterface]
    public class ParticlePoolServiceManager : APoolService<ParticlePool, ParticlePoolContext, ParticlePoolGroup>
    {
        protected override ParticlePool InitializePool(IPoolContext pContext)
        {
            ParticlePoolContext typedContext = (ParticlePoolContext) pContext;

            GameObject obj = Instantiate(typedContext.Source);
            obj.name = typedContext.Id;
            return obj.GetComponent<ParticlePool>();
        }
    }

    [System.Serializable]
    public struct ParticlePoolContext : IPoolContext
    {
        [SerializeField] private string _Id;
        public string Id => _Id;
        [SerializeField] private ParticlePool _Source;
        public GameObject Source => _Source.gameObject;

        [SerializeField] private string _GroupId;
        public string GroupId => _GroupId;
        [SerializeField] private float _GroupWeight;
        public float GroupWeight => _GroupWeight;
    }
}
