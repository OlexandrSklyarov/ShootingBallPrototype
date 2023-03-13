
using System;
using Gameplay.Ball;

namespace Gameplay.Player.FSM.States
{
    public class PushProjectileState : BasePlayerState
    {
        public PushProjectileState(IPlayerContextSwitcher context, IPlayer agent) : base(context, agent)
        {
        }


        public override void OnStart()
        {
            if (_agent.CurrentProjectile == null)
            {
                SwitchToWait();
            }
            else
            {
                _agent.CurrentProjectile.HitEvent += SwitchToCheckResult;
            }
        }

        public override void OnStop()
        {
        }


        private void SwitchToWait() => _context.SwitchState<WaitState>();


        private void SwitchToCheckResult(IEnergyBall projectile) 
        {
            projectile.HitEvent -= SwitchToCheckResult;
            _agent.CurrentProjectile = null;
            _context.SwitchState<CheckResultState>();
        }
    }
}