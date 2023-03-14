using UnityEngine;

namespace Gameplay.Environment
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    public class DoorController : MonoBehaviour
    {
        private bool _isOpened;


        public void Init()
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BoxCollider>().isTrigger = true;
        }

        public void Open()
        {
            if (_isOpened) return;

            _isOpened = true;

            Util.Debug.Print("open door");
        }
    }
}