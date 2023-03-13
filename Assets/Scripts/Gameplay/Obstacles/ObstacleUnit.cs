using System;
using System.Collections;
using UnityEngine;

namespace Gameplay.Obstacles
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class ObstacleUnit : MonoBehaviour, IObstacle
    {
        [SerializeField] private MeshRenderer _viewRenderer;

        public event Action<ObstacleUnit, float, Color> InfectedEvent;


        public void Kill(Color endColor)
        {
            StartCoroutine(DieWithDelay(1f, endColor));
        }


        private IEnumerator DieWithDelay(float delay, Color end)
        {
            for (float time = 0; time < delay; time += Time.deltaTime)
            {
                _viewRenderer.material.color = Color.Lerp(_viewRenderer.material.color, end, time);
                yield return null;
            }

            gameObject.SetActive(false);
        }


        void IObstacle.Infect(float sourceRadius, Color infectedColor)
        {
            InfectedEvent?.Invoke(this, sourceRadius, infectedColor);
        }
    }
}