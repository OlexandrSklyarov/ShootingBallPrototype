
namespace Gameplay.Player.FSM.States
{
    public class CompletedState : BasePlayerState
    {
        public CompletedState(IPlayerContextSwitcher context, IPlayer agent) : base(context, agent)
        {
        }


        public override void OnStart()
        {
            Util.Debug.PrintColor("Finish", UnityEngine.Color.green);
        }
        

        public override void OnStop()
        {
        }
    }
}