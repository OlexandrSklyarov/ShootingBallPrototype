using UnityEngine;

namespace Gameplay.Environment
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    public class DoorController : MonoBehaviour
    {
        [SerializeField] private GameObject _doorView;
        [SerializeField] private float _height = -5f;

        private bool _isOpened;


        public void Init()
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BoxCollider>().isTrigger = true;
        }

        public void Open()
        {
            if (_isOpened) return;

            LeanTween.moveLocalY(_doorView, _height, 0.5f);
            _isOpened = true;
        }
    }
}