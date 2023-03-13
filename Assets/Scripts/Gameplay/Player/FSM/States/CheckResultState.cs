
using System;
using UnityEngine;

namespace Gameplay.Player.FSM.States
{
    public class CheckResultState : BasePlayerState
    {
        public CheckResultState(IPlayerContextSwitcher context, IPlayer agent) : base(context, agent)
        {
        }


        public override void OnStart()
        {
            if (IsPathFree())
                _context.SwitchState<MoveToGateState>();
            else
                _context.SwitchState<WaitState>();
        }
        

        public override void OnStop()
        {
        }


        private bool IsPathFree()
        {
            return !Physics.SphereCast
            (
                new Ray(_agent.MainBall.Center, _agent.MainBall.transform.forward),
                _agent.MainBall.Radius, 
                _agent.Config.ObstaclesLayerMask
            );
        }
    }
}