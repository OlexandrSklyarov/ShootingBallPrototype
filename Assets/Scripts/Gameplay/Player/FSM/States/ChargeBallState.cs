using Common.Input;
using Gameplay.Ball;

namespace Gameplay.Player.FSM.States
{
    public class ChargeBallState : BasePlayerState
    {
        private IEnergyBall _projectile;


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

                    PushBall();
                    
                    break;
            }           
        }


        private void SpawnProjectile()
        {
            _projectile = _agent.Factory.GetProjectile
            (
                _agent.MainBall.GetSpawnPoint(), 
                _agent.MainBall.transform.forward
            );

            _projectile.SetViewSize(_agent.Config.BallSize.Min);
        }


        private void PushBall()
        {
            if (_projectile == null) return;
            
            _projectile.Push(_agent.Config.BaseProjectileVelocity);
            _projectile = null;
            _context.SwitchState<CheckResultState>();
        }


        private void ChargeBallProcess()
        {
            var minSize = _agent.Config.BallSize.Min;
            var info = _agent.MainBall
                .GetEnergy(_agent.Config.EnergyTransferRate * _agent.Config.EnergyTransferSpeed, minSize);
            
            _projectile.AddSize(info.energy);

            Util.Debug.PrintColor($"info.energy {info.energy}", UnityEngine.Color.cyan);

            if (info.currentSize <= minSize) Die();
        }


        private void Die()
        {
            _projectile = null;
            _agent.Die();
            _context.SwitchState<StopState>();
        }
    }
}