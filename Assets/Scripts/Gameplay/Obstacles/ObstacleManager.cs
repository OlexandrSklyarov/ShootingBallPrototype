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
        private readonly ObstacleData _config;
        private readonly List<ObstacleUnit> _obstacles;


        public ObstacleManager(ObstacleData config)
        {
            _config = config;
            _obstacles = new List<ObstacleUnit>();
        }


        public void SpawnObstacles(Vector3 origin, Action onCompleted) => SpawnObstaclesInternal(origin, onCompleted);


        private void SpawnObstaclesInternal(Vector3 origin, Action onCompleted)
        {
            const float HEIGHT = 100f;

            var count = UnityEngine.Random.Range(_config.SpawnCount.Min, _config.SpawnCount.Max);
            var container = new GameObject("[OBSTACLES]").transform;

            while (count > 0)
            {
                var rnd = UnityEngine.Random.insideUnitCircle * _config.SpawnRadius.Max;

                if (rnd.magnitude < _config.SpawnRadius.Min) continue;

                var start = origin + new Vector3(rnd.x, HEIGHT, rnd.y);

                if (Physics.Raycast(start, Vector3.down, out var hit, HEIGHT + 1f, _config.GroundLayerMask))
                {
                    _obstacles.Add(CreateUnit(hit.point, _config.ObstaclePrefab, container));
                    count--;
                }
            }

            onCompleted?.Invoke();
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
            RoutineManager.Run(FindInfected(infectedUnit, infectedRadius, (allInfected) =>
            {
                RoutineManager.Run(KillInfected(allInfected, infectedColor));
            }));
        }


        private IEnumerator KillInfected(List<ObstacleUnit> infected, Color infectedColor)
        {
            foreach (var unit in infected)
            {
                if (!_obstacles.Contains(unit)) continue;

                unit.InfectedEvent -= OnUnitInfected;
                unit.Kill(infectedColor);
                _obstacles.Remove(unit);

                yield return null;
            }
        }


        private IEnumerator FindInfected(ObstacleUnit firstInfestedUnit, float infectedRadius, Action<List<ObstacleUnit>> onCompleted)
        {
            var sqRadius = infectedRadius * infectedRadius;
            var infected = new List<ObstacleUnit>();
            var temp = new Queue<ObstacleUnit>();
            temp.Enqueue(firstInfestedUnit);

            while (temp.Count > 0)
            {
                var cur = temp.Peek();                

                foreach (var other in _obstacles)
                {
                    if (cur == other) continue;
                    if (other.IsInfected) continue;
                    if (temp.Contains(other)) continue;
                    if (infected.Contains(other)) continue;
                    if ((cur.transform.position - other.transform.position).sqrMagnitude > sqRadius) continue;

                    temp.Enqueue(other); 
                }
                
                var first = temp.Dequeue();
                first.Infected();
                infected.Add(first);
                yield return null;
            }

            onCompleted?.Invoke(infected);
        }
    }
}