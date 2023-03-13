using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Player.FSM.States
{
    public class CheckResultState : BasePlayerState
    {
        private CancellationTokenSource _cts;

        public CheckResultState(IPlayerContextSwitcher context, IPlayer agent) : base(context, agent)
        {
        }


        public override void OnStart()
        {
            _cts = new CancellationTokenSource();

            CheckPathFreeAsync
            (
                _cts.Token,
                () => _context.SwitchState<MoveToGateState>(),
                () =>  _context.SwitchState<WaitState>()
            );  
        }
        

        public override void OnStop()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }


        private async void CheckPathFreeAsync(CancellationToken token, Action success, Action failure)
        {
            await Task.Yield();
            if (token.IsCancellationRequested) return;

            var isFree =  !Physics.SphereCast
            (
                new Ray(_agent.MainBall.Center, _agent.MainBall.Forward),
                _agent.MainBall.Radius, 
                float.MaxValue,
                _agent.Config.ObstaclesLayerMask
            );

            Util.Debug.PrintColor($"path isFree: {isFree}", (isFree) ? UnityEngine.Color.green : UnityEngine.Color.red);

            if (isFree)
            {
                success?.Invoke();
                return;
            }

            failure?.Invoke();
        }
    }
}