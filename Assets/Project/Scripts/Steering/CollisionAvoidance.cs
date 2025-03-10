﻿using UnityEngine;

namespace Project.Scripts.Steering
{
    public class CollisionAvoidance : Base
    {
        [SerializeField] private float maxAvoidanceForce = 100f;
        [SerializeField] private float detectionRadius = 10f;
        [SerializeField] private LayerMask obstacleMask;

        private Controller controller;

        private void Start()
        {
            controller = GetComponent<Controller>();
        }

        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();
            var obstacles = Physics.OverlapSphere(transform.position, detectionRadius, obstacleMask);
            obstacles = System.Array.FindAll(obstacles, obstacle => obstacle.gameObject != gameObject);
            if (obstacles.Length == 0) return result;

            var totalAvoidanceForce = Vector3.zero;
            var obstacleCount = 0;

            foreach (var obstacle in obstacles)
            {
                var relativePosition = obstacle.transform.position - transform.position;
                var distance = relativePosition.magnitude;

                if (!(distance > 0.01f)) continue;
                var avoidanceForce = (transform.position - obstacle.transform.position).normalized * (maxAvoidanceForce / distance);
                totalAvoidanceForce += avoidanceForce;
                obstacleCount++;
            }

            if (obstacleCount <= 0) return result;
            totalAvoidanceForce = totalAvoidanceForce.normalized * (maxAvoidanceForce * (1 + obstacleCount * 0.5f));
            result.linear = totalAvoidanceForce;

            return result;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}
