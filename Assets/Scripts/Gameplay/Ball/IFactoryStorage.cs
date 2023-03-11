using UnityEngine;

namespace Gameplay.Ball
{
    public interface IFactoryStorage<T> where T : MonoBehaviour      
    {
        void Reclaim(T item);
    }
}