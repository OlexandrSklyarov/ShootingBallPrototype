using Cinemachine;
using UnityEngine;

namespace Common.Camera
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraAutoFOV : MonoBehaviour
    {
        [SerializeField, Min(1f)] private float _targetFOV = 32f;


        private void Awake() => OnValidate();


        private void OnValidate() 
        {
            var vc = GetComponent<CinemachineVirtualCamera>();

            vc.m_Lens.FieldOfView = Util.CameraExtension
                .CalculateTargetFOV(_targetFOV, UnityEngine.Camera.main.aspect);
        }
    }
}