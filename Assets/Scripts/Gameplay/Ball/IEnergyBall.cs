using System;

namespace Gameplay.Ball
{
    public interface IEnergyBall
    {
        event Action HitEvent;

        void AddSize(float size);
        void SetViewSize(float size);
        void Push(float velocity);
        void OnUpdate();
    }
}