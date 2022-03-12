using System.Collections.Generic;
using UnityEngine;

namespace JvLib.Pooling.Particles
{
    public class ParticlePoolGroup : APoolGroup<ParticlePool>
    {
        /// <summary>
        /// Creates a Burst of a a single type of particle contained in this group
        /// </summary>
        public void BurstSingle(Vector3 pPosition, Color pColor, int pAmount)
        {
            if (Count > 0)
                GetRandomPool().Burst(pPosition, pColor, pAmount);
        }

        /// <summary>
        /// Creates a Burst of a a single type of particle contained in this group
        /// </summary>
        public void BurstSingle(Vector3 pPosition, Color pColor, int pMinAmount, int pMaxAmount) =>
            BurstSingle(pPosition, pColor, Random.Range(pMinAmount, pMaxAmount + 1));

        /// <summary>
        /// Creates a Burst of a random particle contained in this group
        /// </summary>
        public void BurstRandom(Vector3 pPosition, Color pColor, int pAmount)
        {
            if (Count <= 0)
                return;

            int[] counts = new int[Count];
            for (int i = 0; i < pAmount; i++)
                counts[Random.Range(0, Count)]++;

            int index = 0;
            using IEnumerator<ParticlePool> enumerator = GetEnumerator();
            while (enumerator.MoveNext())
            {
                int c = counts[index++];
                if (enumerator.Current != null && c > 0)
                    enumerator.Current.Burst(pPosition, pColor, c);
            }
        }

        /// <summary>
        /// Creates a Burst of a random particle contained in this group
        /// </summary>
        public void BurstRandom(Vector3 pPosition, Color pColor, int pMinAmount, int pMaxAmount) =>
            BurstRandom(pPosition, pColor, Random.Range(pMinAmount, pMaxAmount + 1));
    }
}
