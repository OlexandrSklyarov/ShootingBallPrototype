using UnityEngine;
using Common.Input;
using Gameplay.Player;

namespace Gameplay
{
    public class GameStarter : MonoBehaviour
    {
        private TouchInputManager _input;
        private PlayerController _player;
        private bool _isRunning;


        private void Awake() 
        {
            InitLevel();
            InitPlayer();
            StartGame();            
        }        


        private void InitPlayer()
        {
            _input = new TouchInputManager();
            _player = new PlayerController(_input);
        }


        private void InitLevel()
        {
        }


        private void Update()
        {
            if (!_isRunning) return;

            _input?.OnUpdate();
        }


        private void StartGame()
        {
            _input?.OnEnable();
            _player?.Enable();

            _isRunning = true;
        }


        private void StopGame()
        {
            _input?.OnDisable();
            _player?.Disable();

            _isRunning = false;
        }
    }
}
