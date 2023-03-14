using System;
using System.Collections;
using UnityEngine;

namespace Gameplay.Obstacles
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class ObstacleUnit : MonoBehaviour, IObstacle
    {
        public bool IsInfected => _isInfected;

        [SerializeField] private MeshRenderer _viewRenderer;

        private CapsuleCollider _collider;
        private bool _isInfected;


        public event Action<ObstacleUnit, float, Color> InfectedEvent;


        public void Init()
        {
            _collider = GetComponent<CapsuleCollider>();
        }
        

        public void Infected() => _isInfected = true;


        public void Kill(Color endColor)
        {
            _collider.enabled = false;

            StartCoroutine(DieWithDelay(1f, endColor, () =>
            {
                gameObject.SetActive(false); 
            }));           
        }


        private IEnumerator DieWithDelay(float delay, Color end, Action onCompleted)
        {
            for (float time = 0; time < delay; time += Time.deltaTime)
            {
                _viewRenderer.material.color = Color.Lerp(_viewRenderer.material.color, end, time);
                yield return null;
            }    

            onCompleted?.Invoke();  
        }


        void IObstacle.TryInfect(float sourceRadius, Color infectedColor)
        {
            if (_isInfected) return;

            InfectedEvent?.Invoke(this, sourceRadius, infectedColor);
        }
    }
}