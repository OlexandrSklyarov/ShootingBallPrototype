using UnityEngine;

namespace Gameplay.Environment
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    public class DoorController : MonoBehaviour
    {
        public void Init()
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BoxCollider>().isTrigger = true;
        }
    }
}