using System;
using Data.DataStruct;
using Gameplay.Ball;
using UnityEngine;

namespace Services.Data
{
    [CreateAssetMenu(fileName = "MainGameConfig", menuName = "SO/GameData")]
    public class GameData : ScriptableObject
    {
        [field: SerializeField] public PlayerData Player {get; private set;}
        [field: Space(20f), SerializeField] public BallData Ball {get; private set;}
    }


    [Serializable]
    public class PlayerData
    {
        [field: SerializeField, Min(0.01f)] public float EnergyTransferRate {get; private set;} = 0.1f;
        [field: SerializeField, Min(0.01f)] public float EnergyTransferSpeed {get; private set;} = 2f;
        [field: SerializeField, Min(1f)] public float BaseProjectileVelocity {get; private set;} = 2f;
        [field: SerializeField] public RangeFloatValue BallSize {get; private set;}
    }


    [Serializable]
    public class BallData
    {
        [field: SerializeField] public BallController BallPrefab {get; private set;}
        [field: SerializeField] public BallProjectile ProjectilePrefab {get; private set;}
        [field: SerializeField, Min(1)] public int PoolSize {get; private set;} = 8;
    }      
}