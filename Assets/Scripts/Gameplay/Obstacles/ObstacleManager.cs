using System;
using System.Collections;
using System.Collections.Generic;
using Common.Routine;
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

                var start = origin + new Vector3(rnd.x, HEIGHT, rnd.y);

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
            unit.Init();
            unit.InfectedEvent += OnUnitInfected;

            return unit;
        }


        private void OnUnitInfected(ObstacleUnit infectedUnit, float infectedRadius, Color infectedColor)
        {
            RoutineManager.Run(FindInfected(infectedUnit, infectedRadius, infectedColor, (infected) =>
            {
                RoutineManager.Run(KillInfected(infected));
            }));
        }


        private IEnumerator KillInfected(List<ObstacleUnit> infected)
        {
            foreach (var unit in infected)
            {
                if (!_obstacles.Contains(unit)) continue;

                unit.InfectedEvent -= OnUnitInfected;
                unit.Kill();
                _obstacles.Remove(unit);

                yield return null;
            }
        }


        private IEnumerator FindInfected(ObstacleUnit infectedUnit, float infectedRadius, Color infectedColor, Action<List<ObstacleUnit>> onCompleted)
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
                    if (temp.Contains(other)) continue;
                    if (infected.Contains(other)) continue;
                    if ((cur.transform.position - other.transform.position).sqrMagnitude > sqRadius) continue;

                    temp.Enqueue(other); 
                }

                var first = temp.Dequeue();
                first.PrepareToKill(infectedColor);
                infected.Add(first);

                yield return null;
            }

            onCompleted?.Invoke(infected);
        }
    }
}