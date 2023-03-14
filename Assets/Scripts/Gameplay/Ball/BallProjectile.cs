using System;
using Gameplay.Obstacles;
using UnityEngine;

namespace Gameplay.Ball
{   
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
    public class BallProjectile : MonoBehaviour, IEnergyBall
    {
        public float Radius => _view.localScale.y * 0.5f;

        [SerializeField] private Transform _view;
        [SerializeField] private LayerMask _obstacleLayerMask;

        private Transform _tr;
        private SphereCollider _collider;
        private IFactoryStorage<BallProjectile> _storage;
        private Collider[] _results;
        private Color _myColor;
        private float _velocity;
        private bool _isMoved;
        private bool _isInit;

        public event Action HitEvent;

        public void Init(IFactoryStorage<BallProjectile> storage)
        {
            if (!_isInit)
            {
                _tr = transform;   
                _results = new Collider[1];
                _myColor = _view.GetComponent<MeshRenderer>().material.color;
                _collider = GetComponent<SphereCollider>();
                GetComponent<Rigidbody>().isKinematic = true;
            }

            _isInit = true;
            _storage = storage;

            Stop();             
        }


        void IEnergyBall.AddSize(float deltaSize)
        {
            var size = _view.localScale.y;
            var newSize = size + deltaSize;
            this.SetViewSize(newSize);
        }


        public void SetViewSize(float size)
        {
            _view.localScale = Vector3.one * size;
            
            var pos = _view.localPosition;
            pos.y = Radius;
            _view.localPosition = pos;

            _collider.radius = Radius;
        }


        void IEnergyBall.Push(float baseVelocity)
        {
            _velocity = baseVelocity * _view.localScale.magnitude;
            _isMoved = true;
        }


        public void OnUpdate()
        {
            if (!_isMoved) return;

            _tr.position += _tr.forward * _velocity * Time.deltaTime;
        }

        public void OnFixedUpdate()
        {
            if (!_isMoved) return;

            CheckCollision();
        }

        private void Stop()
        {
            _velocity = 0f;
            _isMoved = false;
        }


        private void Hide() => _storage.Reclaim(this);


        private void CheckCollision()
        {
            var count = Physics.OverlapCapsuleNonAlloc
            (
                _tr.position,
                _tr.position,
                Radius,
                _results,
                _obstacleLayerMask
            );

            if (count <= 0) return;
            if ( !_results[0].TryGetComponent(out IObstacle obstacle)) return;            

            HitEvent?.Invoke();

            obstacle.TryInfect(Radius, _myColor);

            Stop();
            Hide();
        }
    }
}