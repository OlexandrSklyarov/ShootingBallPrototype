using UnityEngine;
using Services.Data;
using System.Collections.Generic;

namespace Gameplay.Ball
{
    public class BallFactory : IFactoryStorage<BallProjectile>
    {
        private readonly BallData _config;
        private readonly Stack<BallProjectile> _pool;


        public BallFactory(BallData config)
        {
            _config = config;       
            _pool = new Stack<BallProjectile>();

            GenerateProjectile();
        }


        private void GenerateProjectile()
        {          
            var container = new GameObject("[POOL]").transform;

            for (int i = 0; i < _config.PoolSize; i++)
            {
                var item = Spawn(container.position, container.forward, container);                
                Reclaim(item);
            }
        }


        public IEnergyBall GetProjectile(Vector3 spawnPosition, Vector3 direction)
        {
            return GetProjectileInternal(spawnPosition, direction);
        }


        private BallProjectile GetProjectileInternal(Vector3 spawnPosition, Vector3 direction)
        {
            var item = (_pool.Count == 0) ?
                Spawn(spawnPosition, direction) :
                _pool.Pop();

            item.transform.SetPositionAndRotation(spawnPosition, Quaternion.LookRotation(direction));
            item.gameObject.SetActive(true);
            item.Init(this);

            return item;
        }


        private BallProjectile Spawn(Vector3 spawnPosition, Vector3 direction, Transform parent = null)
        {
            return UnityEngine.Object.Instantiate
            (
                _config.ProjectilePrefab,
                spawnPosition,
                Quaternion.LookRotation(direction),
                parent
            );
        }


        public BallController GetMainBall(Vector3 spawnPosition, Vector3 direction)
        {
            return UnityEngine.Object.Instantiate
            (
                _config.BallPrefab,
                spawnPosition,
                Quaternion.LookRotation(direction)
            );
        }


        public void Reclaim(BallProjectile item)
        {
            item.gameObject.SetActive(false);
            _pool.Push(item);
        }
    }
}