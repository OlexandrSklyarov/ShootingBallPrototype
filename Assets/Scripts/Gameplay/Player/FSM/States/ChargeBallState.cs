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
            _agent.Input.InputTouchEvent += OnInputHandler;
        }
        

        public override void OnStop()
        {
            _agent.Input.InputTouchEvent -= OnInputHandler;
        }


        public override void OnInputHandler(TouchInputManager.InputData data)
        {
            Util.Debug.PrintColor($"input {data.Phase}", UnityEngine.Color.cyan);            
        } 
    }
}