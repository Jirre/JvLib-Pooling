using UnityEngine;
using Random = UnityEngine.Random;

namespace JvLib.Pooling.Particles
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticlePool : MonoBehaviour
    {
        private ParticleSystem _system;
        private ParticleSystem.MainModule _settings;

        private void Awake()
        {
            _system = GetComponent<ParticleSystem>();
            _settings = _system.main;
        }

        /// <summary>
        /// Creates a Burst of Particles
        /// </summary>
        public void Burst(Vector3 pPosition, Color pColor, int pAmount)
        {
            _system.transform.position = pPosition;
            _settings.startColor = pColor;
            _system.Emit(pAmount);
        }

        /// <summary>
        /// Creates a Burst of Particles
        /// </summary>
        public void Burst(Vector3 pPosition, Color pColor, int pMinAmount, int pMaxAmount) =>
            Burst(pPosition, pColor, Random.Range(pMinAmount, pMaxAmount + 1));
    }
}
