using Common.Input;

namespace Gameplay.Player.FSM.States
{
    public class ChargeBallState : BasePlayerState
    {
        public ChargeBallState(IPlayerContextSwitcher context, IPlayer agent) : base(context, agent)
        {
        }


        public override void OnStart()
        {
            SpawnProjectile();
            
            _agent.Input.InputTouchEvent += OnInputHandler;
        }


        public override void OnStop()
        {
            _agent.Input.InputTouchEvent -= OnInputHandler;
        }


        public override void OnInputHandler(TouchInputManager.InputData data)
        {
            switch(data.Phase)
            {
                case UnityEngine.InputSystem.TouchPhase.Moved:
                case UnityEngine.InputSystem.TouchPhase.Stationary:

                    ChargeBallProcess();

                    break;

                case UnityEngine.InputSystem.TouchPhase.Ended:

                    Push();
                    
                    break;
            }           
        }


        private void SpawnProjectile()
        {
            _agent.CreateProjectile();
            _agent.CurrentProjectile.SetViewSize(_agent.Config.BallSize.Min);
        }


        private void Push()
        {
            if (_agent.CurrentProjectile == null) return;
            
            _agent.CurrentProjectile.Push(_agent.Config.BaseProjectileVelocity);
            _context.SwitchState<PushProjectileState>();
        }


        private void ChargeBallProcess()
        {
            var minSize = _agent.Config.BallSize.Min;            
            var rate = _agent.Config.EnergyTransferRate * _agent.Config.EnergyTransferSpeed;
            var info = _agent.MainBall.GetEnergy(rate, minSize);            
            _agent.CurrentProjectile.AddSize(info.energy);

            if (info.currentSize <= minSize) Die();
        }


        private void Die()
        {
            _agent.ResetProjectile();            
            _context.SwitchState<LossState>();
        }
    }
}