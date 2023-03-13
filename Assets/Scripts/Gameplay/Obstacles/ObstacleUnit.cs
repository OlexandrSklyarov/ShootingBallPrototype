using System;
using System.Collections;
using UnityEngine;

namespace Gameplay.Obstacles
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class ObstacleUnit : MonoBehaviour, IObstacle
    {
        [SerializeField] private MeshRenderer _viewRenderer;

        private CapsuleCollider _collider;
        private bool _isInfected;

        public event Action<ObstacleUnit, float, Color> InfectedEvent;


        public void Init()
        {
            _collider = GetComponent<CapsuleCollider>();
        }
        

        public void Kill()
        {
            _collider.enabled = false;
            gameObject.SetActive(false);
        }


        public void PrepareToKill(Color endColor)
        {
            if (_isInfected) return;

            _isInfected = true;
            StartCoroutine(DieWithDelay(1f, endColor));
        }


        private IEnumerator DieWithDelay(float delay, Color end)
        {
            for (float time = 0; time < delay; time += Time.deltaTime)
            {
                _viewRenderer.material.color = Color.Lerp(_viewRenderer.material.color, end, time);
                yield return null;
            }            
        }


        void IObstacle.Infect(float sourceRadius, Color infectedColor)
        {
            if (_isInfected) return;

            InfectedEvent?.Invoke(this, sourceRadius, infectedColor);
        }
    }
}