
using System;

namespace Gameplay.Player.FSM.States
{
    public class MoveToGateState : BasePlayerState
    {
        private float progressThreshold;

        public MoveToGateState(IPlayerContextSwitcher context, IPlayer agent) : base(context, agent)
        {
        }


        public override void OnStart()
        {
            Util.Debug.PrintColor("Movement process...", UnityEngine.Color.yellow);

            progressThreshold = CalculateProgress(_agent.Config.MinDistToTarget);

            LeanTween.move(_agent.MainBall.gameObject, _agent.TargetDoor.transform.position, _agent.Config.MoveToTargetDuration)
                .setOnUpdate( CheckDistanceToTarget )
                .setOnComplete( SwitchToCompletedState );
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
            if (progress < progressThreshold) return;

            _agent.TargetDoor.Open();
        }
        

        private void SwitchToCompletedState() => _context.SwitchState<CompletedState>();

    }
}