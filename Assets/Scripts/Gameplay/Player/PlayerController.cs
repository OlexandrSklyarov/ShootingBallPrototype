using System;
using System.Collections.Generic;
using System.Linq;
using Common.Input;
using Gameplay.Ball;
using Gameplay.Environment;
using Gameplay.Player.FSM;
using Gameplay.Player.FSM.States;
using Services.Data;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerController : IPlayer, IPlayerContextSwitcher
    {
        TouchInputManager IPlayer.Input => _input;
        PlayerData IPlayer.Config => _config;
        BallController IPlayer.MainBall => _mainBal;
        IEnergyBall IPlayer.CurrentProjectile => _currentProjectile;
        DoorController IPlayer.TargetDoor => _targetDoor;

        private readonly TouchInputManager _input;
        private readonly PlayerData _config;
        private readonly BallFactory _factory;
        private readonly BallController _mainBal;
        private readonly DoorController _targetDoor;
        private List<BasePlayerState> _allStates;
        private BasePlayerState _currentState;
        private IEnergyBall _currentProjectile;
        private bool _isActive;

        public event Action LossEvent;
        public event Action WinEvent;
        public event Action ProjectileDestroyEvent;


        public PlayerController(TouchInputManager input, PlayerData config, 
            BallFactory factory, BallController mainBall, DoorController target)
        {
            _input = input;
            _config = config;
            _factory = factory;
            _mainBal = mainBall;
            _targetDoor = target;

            _mainBal.Init(_config.BallSize.Max, _targetDoor.transform.position);

            _allStates =  new List<BasePlayerState>()
            {
                new WaitState(this, this),
                new ChargeBallState(this, this),
                new PushProjectileState(this, this),
                new MoveToGateState(this, this),
                new LossState(this, this),
                new CompletedState(this, this)
            };

            _currentState = _allStates[0];
        }


        public void Enable()
        {
            if (_isActive) return;

            _currentState?.OnStart(); 
            _isActive = true;
        }


        public void Disable()
        {
            if (!_isActive) return;

            _currentState?.OnStop(); 
            _isActive = false;
        }


        public void SwitchState<T>() where T : BasePlayerState
        {
            var state = _allStates.FirstOrDefault(s => s is T);

            _currentState?.OnStop();
            _currentState = state;
            _currentState?.OnStart();
        }


        public void OnUpdate()
        {
            if (!_isActive) return;

            _currentState?.OnUpdate();
        }


        public void OnFixedUpdate()
        {
            if (!_isActive) return;
            
            _currentState?.OnFixedUpdate();
        }


        void IPlayer.CreateProjectile()
        {
            _currentProjectile = _factory.GetProjectile
            (
                _mainBal.GetSpawnPoint(), 
                _mainBal.transform.forward
            );

            _currentProjectile.HitEvent += OnProjectileHit;
        }


        void IPlayer.ResetProjectile()
        {
            if (_currentProjectile == null) return;

            _currentProjectile.HitEvent -= OnProjectileHit;
            _currentProjectile = null;
        }


        private void OnProjectileHit()
        {
            _currentProjectile.HitEvent -= OnProjectileHit;
            _currentProjectile = null;

            ProjectileDestroyEvent?.Invoke();
        }


        void IPlayer.Loss() => LossEvent?.Invoke();


        void IPlayer.Win() => WinEvent?.Invoke();
    }
}