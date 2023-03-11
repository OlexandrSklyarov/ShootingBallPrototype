
namespace Gameplay.Ball
{
    public interface IEnergyBall
    {
        void AddSize(float size);
        void SetViewSize(float size);
        void Push(float velocity);
    }
}