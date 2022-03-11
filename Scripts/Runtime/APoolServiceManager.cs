using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using JvLib.Services;

namespace JvLib.Pooling
{
    public abstract class APoolService<T, C> : MonoBehaviour, IService
        where T : Component
        where C : IPoolData
    {
        [SerializeField] private List<C> _Data;

        protected Dictionary<string, T> _pools;
        protected Dictionary<string, List<IPoolData>> _groups;

        private const string SCENE_NAME = "Pools";
        private Transform _parent;

        private bool _isReady;

        public bool IsReady => _isReady;

        private void Awake()
        {
            ServiceLocator.Instance.Register(this);
            Debug.Log("boop");
            Scene _poolScene = SceneManager.GetSceneByName(SCENE_NAME);
            if (_poolScene.isLoaded)
                return;

            _poolScene = SceneManager.CreateScene(SCENE_NAME);

            GameObject obj = new GameObject(GetType().Name);
            _pools = new Dictionary<string, T>();
            _groups = new Dictionary<string, List<IPoolData>>();

            foreach (IPoolData data in _Data)
            {
                if (_pools.ContainsKey(data.Id))
                {
                    Debug.LogError($"Attempting to add duplicate key in Pool {GetType().Name}: {data.Id}");
                    continue;
                }

                if(!string.IsNullOrWhiteSpace(data.GroupId))
                {
                    if (!_groups.ContainsKey(data.GroupId))
                        _groups.Add(data.GroupId, new List<IPoolData>());
                    _groups[data.GroupId].Add(data);
                }

                T pool = InitializePool(data);
                pool.transform.SetParent(obj.transform);
                pool.transform.localPosition = Vector3.zero;
                pool.transform.localScale = Vector3.one;
                pool.transform.localRotation = Quaternion.identity;

                _pools.Add(data.Id, pool);
            }

            SceneManager.MoveGameObjectToScene(obj, _poolScene);

            _isReady = true;
            ServiceLocator.Instance.ReportInstanceReady(this);
        }

        protected abstract T InitializePool(IPoolData pContext);
    }

    public interface IPoolData
    {
        string Id { get; }
        
        string GroupId { get; }
        float GroupWeight { get; }
    }
}