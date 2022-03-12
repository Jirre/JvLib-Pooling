using System.Collections.Generic;
using UnityEngine;

namespace JvLib.Pooling.Objects
{
    public class ObjectPool : MonoBehaviour
    {
        public string Id { get; private set; }
        public GameObject Source { get; private set; }

        private Queue<PooledObject> _pool;
        private List<PooledObject> _activeList;

#if UNITY_EDITOR
        public int ActiveCount => _activeList?.Count ?? 0;
        public int PassiveCount => _pool?.Count ?? 0;
#endif

        private Transform _activeParent;
        private Transform _passiveParent;

        internal void Initialize(ObjectPoolContext pContext)
        {
            Id = pContext.Id;
            Source = pContext.Source;
            _pool = new Queue<PooledObject>();
            _activeList = new List<PooledObject>();

            _activeParent = new GameObject("Active").transform;
            _passiveParent = new GameObject("Passive").transform;
            _activeParent.SetParent(transform);
            _passiveParent.SetParent(transform);

            if (pContext.Buffer > 0)
                AddNewObject(pContext.Buffer);
        }

        /// <summary>
        /// Activates a new GameObject from this pool at the given location and rotation
        /// </summary>
        public GameObject Activate(Vector3 pPosition, Quaternion pRotation)
        {
            //Create new object if necessary
            if (_pool.Count <= 0)
                AddNewObject();

            //Get object from pool
            PooledObject pooledObject = _pool.Dequeue();
            if (pooledObject == null)
                throw new System.NullReferenceException($"Tried to pull from empty Queue in: {Id}_Pool");

            Transform trans = pooledObject.transform;
            GameObject obj = pooledObject.gameObject;

            trans.position = pPosition;
            trans.rotation = pRotation;
            trans.SetParent(_passiveParent);

            _activeList.Add(pooledObject);
            obj.SetActive(true);
            return obj.gameObject;
        }

        /// <summary>
        /// Deactivates a GameObject that originates from this object pool
        /// </summary>
        public void Deactivate(GameObject pObject)
        {
            PooledObject pooledObject = pObject == null ? null : pObject.GetComponent<PooledObject>();
            if (pooledObject == null)
                return;

            if (pooledObject.Id != Id)
                return;

            GameObject obj = pooledObject.gameObject;
            _activeList.Remove(pooledObject);
            obj.transform.SetParent(_passiveParent);
            _pool.Enqueue(pooledObject);
            obj.transform.position = ObjectPoolServiceManager.PASSIVE_POSITION;

            obj.SetActive(false);
        }

        /// <summary>
        /// Deactivates all active entities from this object pool
        /// </summary>
        public void DeactivateAll()
        {
            if (_activeList.Count == 0) return;
            for (int i = _activeList.Count - 1; i >= 0; i--)
                Deactivate(_activeList[i].gameObject);
        }

        private void AddNewObject(int pCount = 1)
        {
            for (int i = 0; i < pCount; i++)
            {
                GameObject obj = Instantiate(Source, ObjectPoolServiceManager.PASSIVE_POSITION, Quaternion.identity,
                    _passiveParent);
                obj.transform.name = Source.name;
                PooledObject pooledObject = obj.GetComponent<PooledObject>();

                _pool.Enqueue(pooledObject);
                obj.SetActive(false);
            }
        }
    }
}
