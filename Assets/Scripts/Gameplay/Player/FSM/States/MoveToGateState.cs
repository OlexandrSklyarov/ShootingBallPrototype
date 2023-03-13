
namespace Gameplay.Player.FSM.States
{
    public class MoveToGateState : BasePlayerState
    {
        public MoveToGateState(IPlayerContextSwitcher context, IPlayer agent) : base(context, agent)
        {
        }


        public override void OnStart()
        {
            Util.Debug.PrintColor("Movement process...", UnityEngine.Color.green);
        }
        

        public override void OnStop()
        {
        }
    }
}