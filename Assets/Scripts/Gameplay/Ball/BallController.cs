using UnityEngine;

namespace Gameplay.Ball
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] private Transform _view;

        public void Init(float size)
        {
            SetViewSize(size);
        }


        private void SetViewSize(float size)
        {
            _view.localScale = Vector3.one * size;
            
            var pos = _view.localPosition;
            pos.y = _view.localScale.y * 0.5f;
            _view.localPosition = pos;
        }


        public (float energy, float currentSize) GetEnergy(float energyTransferRate, float minSize)
        {
            var size = _view.localScale.y;
            var energy = size - (size - energyTransferRate * Time.deltaTime);
            var newSize = Mathf.Max(size - energy, minSize);


            SetViewSize(newSize);

            return (energy, newSize);
        }


        public Vector3 GetSpawnPoint()
        {
            return transform.position + transform.forward * _view.localScale.y;
        }
    }
}