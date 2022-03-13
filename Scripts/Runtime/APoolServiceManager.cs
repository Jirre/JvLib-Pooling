using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using JvLib.Services;

namespace JvLib.Pooling
{
    /// <summary>
    /// Base Pooling Service
    /// </summary>
    /// <typeparam name="T">Pooling Type</typeparam>
    /// <typeparam name="C">Pooling Context Type</typeparam>
    /// <typeparam name="G">Pooling Group Type</typeparam>
    public abstract class APoolService<T, C, G> : MonoBehaviour, IService
        where T : Component
        where C : IPoolContext
        where G : APoolGroup<T>, new()
    {
        [SerializeField] private List<C> _Data;

        private Dictionary<string, T> _pools;
        private Dictionary<string, G> _groups;

        private const string SCENE_NAME = "Pooling";
        private Transform _parent;

        public bool IsServiceReady { get; private set; }

        private void Awake()
        {
            ServiceLocator.Instance.Register(this);
            Scene scene = SceneManager.GetSceneByName(SCENE_NAME);
            
            if (!scene.isLoaded)
                scene = SceneManager.CreateScene(SCENE_NAME);

            GameObject obj = new GameObject(GetType().Name);
            _pools = new Dictionary<string, T>();
            _groups = new Dictionary<string, G>();

            foreach (C data in _Data)
            {
                if (_pools.ContainsKey(data.Id))
                {
                    Debug.LogError($"Attempting to add duplicate key in Pool {GetType().Name}: {data.Id}");
                    continue;
                }

                T pool = InitializePool(data);
                Transform trans = pool.transform;
                trans.SetParent(obj.transform);
                trans.localPosition = Vector3.zero;
                trans.localScale = Vector3.one;
                trans.localRotation = Quaternion.identity;

                _pools.Add(data.Id, pool);

                if (string.IsNullOrWhiteSpace(data.GroupId)) continue;

                if (!_groups.ContainsKey(data.GroupId))
                    _groups.Add(data.GroupId, new G());
                _groups[data.GroupId].Add(pool, data.GroupWeight);
            }

            SceneManager.MoveGameObjectToScene(obj, scene);

            IsServiceReady = true;
            ServiceLocator.Instance.ReportInstanceReady(this);
        }

        protected abstract T InitializePool(IPoolContext pContext);

        public T GetPool(string pId)
        {
            if (string.IsNullOrWhiteSpace(pId) || _pools == null)
                return null;

            return !_pools.TryGetValue(pId, out T pool) ? null : pool;
        }

        public G GetGroup(string pGroupId)
        {
            if (string.IsNullOrWhiteSpace(pGroupId) || _groups == null)
                return null;

            return !_groups.TryGetValue(pGroupId, out G group) ? null : group;
        }
    }
}
