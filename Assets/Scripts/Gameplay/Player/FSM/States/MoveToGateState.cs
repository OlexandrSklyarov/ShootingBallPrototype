
namespace Gameplay.Player.FSM.States
{
    public class MoveToGateState : BasePlayerState
    {
        private float _progressThreshold;


        public MoveToGateState(IPlayerContextSwitcher context, IPlayer agent) : base(context, agent)
        {
        }


        public override void OnStart()
        {            
            _progressThreshold = CalculateProgress(_agent.Config.MinDistToTarget);               

            _agent.MainBall.JumpTo
            (
                _agent.TargetDoor.transform.position, 
                _agent.Config.MoveToTargetDuration, 
                CheckDistanceToTarget,
                SwitchToCompletedState
            );   
        }
        

        public override void OnStop()
        {
        }


        private float CalculateProgress(float minDistance)
        {
            var distance = (_agent.MainBall.transform.position - _agent.TargetDoor.transform.position).magnitude;
            return 1f - minDistance / distance;
        }


        private void CheckDistanceToTarget(float progress)
        {
            if (progress < _progressThreshold) return;

            _agent.TargetDoor.Open();
        }
        

        private void SwitchToCompletedState() => _context.SwitchState<CompletedState>();

    }
}