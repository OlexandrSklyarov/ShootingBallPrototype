
namespace Gameplay.Player.FSM.States
{
    public class PushProjectileState : BasePlayerState
    {
        public PushProjectileState(IPlayerContextSwitcher context, IPlayer agent) : base(context, agent)
        {
        }


        public override void OnStart()
        {                    
            _agent.ProjectileDestroyEvent += SwitchToCheckResult;   
        }


        public override void OnUpdate()
        {
            _agent.CurrentProjectile?.OnUpdate();
        }


        public override void OnFixedUpdate()
        {
            _agent.CurrentProjectile?.OnFixedUpdate();
        }


        public override void OnStop()
        {            
            _agent.ResetProjectile();
        }


        private void SwitchToCheckResult() 
        {            
            _agent.ProjectileDestroyEvent -= SwitchToCheckResult;   
            _context.SwitchState<CheckResultState>();
        }
    }
}