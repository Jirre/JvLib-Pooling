using System.Collections.Generic;
using UnityEngine;

namespace JvLib.Pooling.Objects
{
    public class ObjectPoolGroup : APoolGroup<ObjectPool>
    {
        public GameObject Activate(Vector3 pPosition, Quaternion pRotation) =>
            Count > 0 ? GetRandomPool().Activate(pPosition, pRotation) : null;

        public void DeactivateAll()
        {
            if (Count <= 0)
                return;

            using IEnumerator<ObjectPool> enumerator = GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current != null)
                    enumerator.Current.DeactivateAll();
            }
        }
    }
}
