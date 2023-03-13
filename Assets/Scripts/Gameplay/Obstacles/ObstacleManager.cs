using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Data;
using UnityEngine;

namespace Gameplay.Obstacles
{
    public class ObstacleManager
    {
        private readonly List<ObstacleUnit> _obstacles;


        public ObstacleManager(ObstacleData config, Vector3 origin)
        {
            _obstacles = new List<ObstacleUnit>();

            SpawnObstacles(config, origin);
        }


        private void SpawnObstacles(ObstacleData config, Vector3 origin)
        {
            const float HEIGHT = 100f;

            var count = UnityEngine.Random.Range(config.SpawnCount.Min, config.SpawnCount.Max);
            var container = new GameObject("[OBSTACLES]").transform;

            while (count > 0)
            {
                var rnd = UnityEngine.Random.insideUnitCircle * config.SpawnRadius.Max;

                if (rnd.magnitude < config.SpawnRadius.Min) continue;

                var start = new Vector3(rnd.x, HEIGHT, rnd.y);

                if (Physics.Raycast(start, Vector3.down, out var hit, HEIGHT + 1f, config.GroundLayerMask))
                {
                    _obstacles.Add(CreateUnit(hit.point, config.ObstaclePrefab, container));
                    count--;
                }
            }
        }


        private ObstacleUnit CreateUnit(Vector3 point, ObstacleUnit prefab, Transform container)
        {
            var unit = UnityEngine.Object.Instantiate(prefab, point, Quaternion.identity, container);
            unit.InfectedEvent += OnUnitInfected;

            return unit;
        }


        private void OnUnitInfected(ObstacleUnit infectedUnit, float infectedRadius, Color infectedColor)
        {
            var infected = FindNearInfectedUnits(infectedUnit, infectedRadius);

            for(var i = 0; i < infected.Length; i++)
            {
                var unit = infected[i];

                if (_obstacles.Contains(unit))
                {
                    unit.Kill(infectedColor);
                    unit.InfectedEvent -= OnUnitInfected;
                    _obstacles.Remove(unit);
                }
            }
        }


        private ObstacleUnit[] FindNearInfectedUnits(ObstacleUnit infectedUnit, float infectedRadius)
        {
            var sqRadius = infectedRadius * infectedRadius;

            var infected = new List<ObstacleUnit>();
            var temp = new Queue<ObstacleUnit>();
            temp.Enqueue(infectedUnit);            

            while (temp.Count > 0)
            {
                var cur = temp.Peek();

                foreach (var other in _obstacles)
                {
                    if (cur == other) continue;

                    var sqDist = (cur.transform.position - other.transform.position).sqrMagnitude;

                    if (sqDist <= sqRadius && !infected.Contains(other))
                    {
                        temp.Enqueue(other);
                    }
                }  

                infected.Add(temp.Dequeue()); 
            }

            return infected.ToArray();
        }
    }
}