using UnityEngine;
using Common.Input;
using Gameplay.Player;
using Services.Data;
using Gameplay.Ball;

namespace Gameplay
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private GameData _config;
        [SerializeField] private Transform _mainBallSpawnPoint;
        
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
            var factory = new BallFactory(_config.Ball);
            var mainBall = factory.GetMainBall(_mainBallSpawnPoint.position, _mainBallSpawnPoint.forward);

            _player = new PlayerController(_input, _config.Player, factory, mainBall);
        }


        private void InitLevel()
        {
        }


        private void Update()
        {
            if (!_isRunning) return;

            _input?.OnUpdate();
            _player?.OnUpdate();
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
