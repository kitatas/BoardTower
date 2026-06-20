using UnityEngine;

namespace BoardTower.Common.Presentation.View
{
    public sealed class TapEffectView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particleSystem = default;

        public float duration
        {
            get
            {
                var main = particleSystem.main;
                return main.duration + main.startLifetime.constantMax;
            }
        }

        public void SetUp(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
            Stop();
            particleSystem.Play(true);
        }

        public void Rent()
        {
            gameObject.SetActive(true);
        }

        public void Release()
        {
            Stop();
            gameObject.SetActive(false);
        }

        private void Stop()
        {
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}