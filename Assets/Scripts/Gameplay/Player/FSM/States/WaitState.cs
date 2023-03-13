using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Input;
using UnityEngine;

namespace Gameplay.Player.FSM.States
{
    public class WaitState : BasePlayerState
    {
        private CancellationTokenSource _cts;

        private const float CHECK_FREE_PATH_DELAY = 0.2f;

        public WaitState(IPlayerContextSwitcher context, IPlayer agent) : base(context, agent)
        {
        }


        public override void OnStart()
        {
            _agent.Input.InputTouchEvent += OnInputHandler;

            _cts = new CancellationTokenSource();

            CheckPathFreeAsync
            (
                CHECK_FREE_PATH_DELAY,
                _cts.Token,
                () => _context.SwitchState<MoveToGateState>()
            );
        }


        public override void OnStop()
        {
            _agent.Input.InputTouchEvent -= OnInputHandler;

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }


        public override void OnInputHandler(TouchInputManager.InputData data)
        {
            _context.SwitchState<ChargeBallState>();
        }


        private async void CheckPathFreeAsync(float checkDelay, CancellationToken token, Action success)
        {
            await Task.Delay(TimeSpan.FromSeconds(checkDelay));
            if (token.IsCancellationRequested) return;

            var isFree = false;

            while (!isFree)
            {
                await Task.Delay(TimeSpan.FromSeconds(checkDelay));
                if (token.IsCancellationRequested) return;

                isFree = !Physics.SphereCast
                (
                    new Ray(_agent.MainBall.Center, _agent.MainBall.Forward),
                    _agent.MainBall.Radius,
                    (_agent.MainBall.transform.position - _agent.TargetDoor.transform.position).magnitude,
                    _agent.Config.ObstacleLayerMask
                );
            }

            success?.Invoke();
        }
    }
}