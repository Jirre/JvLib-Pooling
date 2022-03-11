using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JvLib.Services;

namespace JvLib.Pooling.Objects
{
    [ServiceInterface]
    public class ObjectPoolServiceManager : APoolService<ObjectPoolManager, ObjectPoolData>
    {
        protected override ObjectPoolManager InitializePool(IPoolData pContext)
        {
            return new GameObject(pContext.Id).AddComponent<ObjectPoolManager>();
        }
    }

    [System.Serializable]
    public struct ObjectPoolData : IPoolData
    {
        [SerializeField] private string _Id;
        public string Id => _Id;
        [SerializeField] private GameObject _Source;
        public GameObject Source => _Source;
        [SerializeField] private int _Buffer;
        public int Buffer => _Buffer;

        [SerializeField] private string _GroupId;
        public string GroupId => _GroupId;
        [SerializeField] private float _GroupWeight;
        public float GroupWeight => _GroupWeight;
    }
}