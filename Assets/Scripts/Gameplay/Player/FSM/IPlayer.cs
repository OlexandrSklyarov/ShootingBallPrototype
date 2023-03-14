using System;
using Common.Input;
using Gameplay.Ball;
using Gameplay.Environment;
using Services.Data;

namespace Gameplay.Player.FSM
{
    public interface IPlayer
    {       
        TouchInputManager Input {get;}
        PlayerData Config {get;}
        BallController MainBall {get;}
        IEnergyBall CurrentProjectile {get;}
        DoorController TargetDoor {get;}

        event Action ProjectileDestroyEvent;

        void CreateProjectile();
        void ResetProjectile();
        void Loss();
        void Win();
    }
}