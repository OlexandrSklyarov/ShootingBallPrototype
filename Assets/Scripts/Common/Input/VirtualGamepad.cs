using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Common.Input
{
    public class VirtualGamepad : MonoBehaviour, IGamepad
    {
        [SerializeField] private VirtualStick _stick;
        [SerializeField] private bool _isUseAcrossScreen;
        
        private TouchControls _touchControls;
        private bool _isCheckedInput;

        public event Action<Vector2> StickInputEvent;


        public void Init(TouchControls touchControls)
        {
            _touchControls = touchControls;
            
            var referenceResolution = GetComponent<CanvasScaler>().referenceResolution;
            _stick.Init(referenceResolution);
            if (_isUseAcrossScreen) _stick.Hide();
                
            _touchControls.Enable();
            
            _touchControls.Touch.TouchPress.started += OnStarted;
            _touchControls.Touch.TouchPress.canceled += OnCanceled;
        }
        
        
        private void OnDestroy()
        {
            _touchControls.Touch.TouchPress.started -= OnStarted;
            _touchControls.Touch.TouchPress.canceled -= OnCanceled;
            
            _touchControls?.Disable();
            _isCheckedInput = false;
        }


        public void Activate()
        {
            _isCheckedInput = true;
        }
        
        
        public void Deactivate()
        {
            _isCheckedInput = false;
            StickInputEvent?.Invoke(Vector2.zero);
            _stick.Hide();
        }
        
        
        private void OnStarted(InputAction.CallbackContext ctx)
        {
            if (!_isUseAcrossScreen) return;
            if (!_isCheckedInput) return;
            
            var position = _touchControls.Touch.TouchPosition.ReadValue<Vector2>();
            _stick.SetScreenPosition(position);
            _stick.Show();
        }
        
        
        private void OnCanceled(InputAction.CallbackContext ctx)
        {
            if (!_isUseAcrossScreen) return;
            if (!_isCheckedInput) return;

            _stick.Hide();
        }


        public void OnUpdate()
        {
            if (!_isCheckedInput) return;
          
            StickInputEvent?.Invoke(_touchControls.Gamepad.MoveStick.ReadValue<Vector2>());            
        }
    }
}