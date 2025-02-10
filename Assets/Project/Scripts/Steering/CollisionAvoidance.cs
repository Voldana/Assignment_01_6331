using UnityEngine;

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

            if (obstacles.Length == 0) return result; // No obstacles detected

            Transform closestObstacle = null;
            var closestDistance = float.MaxValue;

            foreach (var obstacle in obstacles)
            {
                if (obstacle.gameObject == gameObject) continue; // Ignore itself

                var relativePosition = obstacle.transform.position - transform.position;
                var distance = relativePosition.magnitude;

                // Find the closest obstacle
                if (!(distance < closestDistance)) continue;
                closestDistance = distance;
                closestObstacle = obstacle.transform;
            }

            if (!closestObstacle) return result; // No collision risks

            // ✅ Use position-based avoidance instead of velocity calculations
            var avoidanceForce = (transform.position - closestObstacle.position).normalized * maxAvoidanceForce;
            result.linear = avoidanceForce;

            return result;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}
