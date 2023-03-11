using System.Collections.Generic;
using System.Linq;
using Common.Input;
using Gameplay.Player.FSM;
using Gameplay.Player.FSM.States;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerController : IPlayer, IPlayerContextSwitcher
    {
        TouchInputManager IPlayer.Input => _input;

        readonly TouchInputManager _input;
        private List<BasePlayerState> _allStates;
        private BasePlayerState _currentState;
        private bool _isActive;


        public PlayerController(TouchInputManager input)
        {
            _input = input;

            _allStates =  new List<BasePlayerState>()
            {
                new WaitState(this, this),
                new ChargeBallState(this, this),
                new ShootState(this, this),
                new CheckResultState(this, this),
                new MoveToGateState(this, this)
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
    }
}