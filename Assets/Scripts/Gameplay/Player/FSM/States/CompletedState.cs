
namespace Gameplay.Player.FSM.States
{
    public class CompletedState : BasePlayerState
    {
        public CompletedState(IPlayerContextSwitcher context, IPlayer agent) : base(context, agent)
        {
        }


        public override void OnStart()
        {
            _agent.Win();
        }
        

        public override void OnStop()
        {
        }
    }
}