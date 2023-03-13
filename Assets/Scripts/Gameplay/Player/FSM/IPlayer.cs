using Common.Input;
using Gameplay.Ball;
using Services.Data;

namespace Gameplay.Player.FSM
{
    public interface IPlayer
    {       
        TouchInputManager Input {get;}
        PlayerData Config {get;}
        BallFactory Factory {get;}
        BallController MainBall {get;}
        IEnergyBall CurrentProjectile {get; set;}

        void Die();
    }
}