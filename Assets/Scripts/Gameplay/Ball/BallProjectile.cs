using UnityEngine;

namespace Gameplay.Ball
{   
    public class BallProjectile : MonoBehaviour, IEnergyBall
    {
        [SerializeField] private Transform _view;


        private Transform _tr;
        private float _velocity;
        private IFactoryStorage<BallProjectile> _storage;
        private bool _isMoved;


        private void Awake() 
        {
            _tr = transform;   
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
            pos.y = _view.localScale.y * 0.5f;
            _view.localPosition = pos;
        }


        void IEnergyBall.Push(float baseVelocity)
        {
            _velocity = baseVelocity * _view.localScale.magnitude;
            _isMoved = true;
        }


        private void Update()
        {
            if (!_isMoved) return;

            _tr.position += _tr.forward * _velocity * Time.deltaTime;
        }


        public void Stop()
        {
            _velocity = 0f;
            _isMoved = false;
        }


        private void Hide() => _storage.Reclaim(this);


        public void Setup(IFactoryStorage<BallProjectile> storage)
        {
            _storage = storage;
        }
    }
}