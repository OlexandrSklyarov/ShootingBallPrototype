using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Ball
{
    public class BallController : MonoBehaviour
    {
        public Vector3 Center => _view.position;
        public float Radius => Diameter * 0.5f;
        private float Diameter => _view.localScale.y;
        public Vector3 Forward => transform.forward;


        [SerializeField] private Transform _view;
        [SerializeField] private Transform _roadView;

        private float _roadDistance;


        public void Init(float size, Vector3 targetPosition)
        {
            _roadDistance = (targetPosition - transform.position).magnitude;
            SetViewSize(size);
        }


        private void SetViewSize(float size)
        {
            _view.localScale = Vector3.one * size;
            
            var pos = _view.localPosition;
            pos.y = Radius;
            _view.localPosition = pos;

            SetRoadWidth();
        }

        private void SetRoadWidth()
        {
            _roadView.localScale = new Vector3(Diameter, 1f, _roadDistance);
        }


        public (float energy, float currentSize) GetEnergy(float energyTransferRate, float minSize)
        {
            var size = Diameter;
            var energy = size - (size - energyTransferRate * Time.deltaTime);
            var newSize = Mathf.Max(size - energy, minSize);

            SetViewSize(newSize);

            return (energy, newSize);
        }


        public Vector3 GetSpawnPoint()
        {
            return transform.position + transform.forward * _view.localScale.y;
        }        


        private void HideRoadView() => _roadView.gameObject.SetActive(false);


        public void JumpTo(Vector3 targetPosition, float duration, Action<float> onProgress, Action onCompleted)
        {
            HideRoadView();

            LeanTween
                .move(this.gameObject, targetPosition, duration)
                .setOnUpdate( onProgress )
                .setOnComplete( onCompleted );           
            
            var startLocalPosition = _view.localPosition;

            _view.DOLocalJump(Vector3.up, 4f, 5, duration).OnComplete(() => 
            {
                _view.DOLocalMove(startLocalPosition, 1f);
            });
        }
    }
}