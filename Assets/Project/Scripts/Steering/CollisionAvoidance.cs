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

            if (obstacles.Length == 0) return result;  // No obstacles detected

            Transform closestObstacle = null;
            var closestTimeToCollision = float.MaxValue;
            var closestRelativeVelocity = Vector3.zero;
            var closestRelativePosition = Vector3.zero;
            var closestDistance = float.MaxValue;

            foreach (var obstacle in obstacles)
            {
                if (obstacle.gameObject == gameObject) continue; 

                var relativeVelocity = Vector3.zero;
                if (obstacle.TryGetComponent<Controller>(out var tarController))
                {
                    relativeVelocity = tarController.GetVelocity() - controller.GetVelocity();
                }

                var relativePosition = transform.position - obstacle.transform.position;  // ✅ Flipped to push away
                var distance = relativePosition.magnitude;

                float timeToCollision;
                
                if (relativeVelocity.sqrMagnitude > 0.01f)
                {
                    timeToCollision = Vector3.Dot(relativePosition, relativeVelocity) / relativeVelocity.sqrMagnitude;
                }
                else
                {
                    timeToCollision = 0; // ✅ Treat stationary objects as an immediate threat
                }

                // ✅ Prioritize static objects based on distance
                if (relativeVelocity.sqrMagnitude < 0.01f && distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObstacle = obstacle.transform;
                    closestRelativePosition = relativePosition;
                }
                else if (timeToCollision >= 0 && timeToCollision < closestTimeToCollision)
                {
                    closestTimeToCollision = timeToCollision;
                    closestObstacle = obstacle.transform;
                    closestRelativeVelocity = relativeVelocity;
                    closestRelativePosition = relativePosition;
                }
            }

            if (!closestObstacle) return result; // No collision risks

            // ✅ Properly calculate avoidance force (reversed direction)
            var avoidanceForce = (transform.position - closestObstacle.position).normalized * maxAvoidanceForce;

            if (closestRelativeVelocity.sqrMagnitude < 0.01f) // Static object avoidance
            {
                avoidanceForce = closestRelativePosition.normalized * (maxAvoidanceForce * 2);
            }

            result.linear = avoidanceForce;
            return result;
        }
    }
}
