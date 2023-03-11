using System;
using System.Collections.Generic;
using System.Linq;
using Common.Input;
using Gameplay.Ball;
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
        BallFactory IPlayer.Factory => _factory;
        BallController IPlayer.MainBall => _mainBal;

        private readonly TouchInputManager _input;
        private readonly PlayerData _config;
        private readonly BallFactory _factory;
        private readonly BallController _mainBal;

        private List<BasePlayerState> _allStates;
        private BasePlayerState _currentState;
        private bool _isActive;

        public event Action DieEvent;


        public PlayerController(TouchInputManager input, PlayerData config, 
            BallFactory factory, BallController mainBall)
        {
            _input = input;
            _config = config;
            _factory = factory;
            _mainBal = mainBall;

            _mainBal.Init(_config.BallSize.Max);

            _allStates =  new List<BasePlayerState>()
            {
                new WaitState(this, this),
                new ChargeBallState(this, this),
                new CheckResultState(this, this),
                new MoveToGateState(this, this),
                new StopState(this, this)
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

            Util.Debug.PrintColor($"player state {_currentState.GetType()}", Color.cyan);
        }


        public void OnUpdate()
        {
            if (!_isActive) return;

            _currentState?.OnUpdate();
        }


        void IPlayer.Die() => DieEvent?.Invoke();
    }
}