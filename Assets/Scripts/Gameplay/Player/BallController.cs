using UnityEngine;

namespace Gameplay.Player
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] private Transform _view;


        public void SetViewSize(float size)
        {
            _view.localScale = Vector3.one * size;
            
            var pos = _view.localPosition;
            pos.y = _view.localScale.y;
            _view.localPosition = pos;
        }
    }
}