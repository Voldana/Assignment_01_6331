using UnityEngine;

namespace Project.Scripts.Steering
{
    public class Wander : Base
    {
        [SerializeField] private Transform islandCenter; // Center of the island
        [SerializeField] private float wanderRadius = 30f; // Max distance from island
        [SerializeField] private float wanderSpeed = 5f; // Speed of wandering
        [SerializeField] private float turnSpeed = 3f; // Controls smooth turning
        [SerializeField] private float targetChangeThreshold = 3f; // Distance threshold to change target
        [SerializeField] private float rayDistance = 5f; // Ray distance for obstacle detection
        [SerializeField] private LayerMask obstacleMask; // Layer mask for obstacles

        private Vector3 currentTarget;

        private void Start()
        {
            if (islandCenter == null)
            {
                Debug.LogError("IslandCenter not assigned for WanderAroundIsland!");
                return;
            }

            SetNewTarget();
        }

        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();

            // 1️⃣ Check for obstacles or edge encounters
            if (DetectObstacleOrEdge(out var avoidanceTarget))
            {
                currentTarget = avoidanceTarget;
            }

            // 2️⃣ Move toward the current target
            var directionToTarget = (currentTarget - transform.position).normalized;

            // Rotate smoothly toward the target
            var desiredRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * turnSpeed);

            // Apply forward movement
            result.linear = transform.forward * wanderSpeed;

            // 3️⃣ Change target if close to the current one
            if (Vector3.Distance(transform.position, currentTarget) < targetChangeThreshold)
            {
                SetNewTarget();
            }

            return result;
        }

        // Detects obstacles or approaching the island edge
        private bool DetectObstacleOrEdge(out Vector3 avoidanceTarget)
        {
            avoidanceTarget = Vector3.zero;

            // Ray directions (forward, left, right)
            Vector3[] directions = {
                transform.forward,
                Quaternion.Euler(0, -25, 0) * transform.forward,
                Quaternion.Euler(0, 25, 0) * transform.forward
            };

            foreach (var direction in directions)
            {
                if (!Physics.Raycast(transform.position, direction, out RaycastHit hit, rayDistance,
                        obstacleMask)) continue;
                // Calculate new target using normal vector of the hit
                var hitNormal = hit.normal;
                avoidanceTarget = transform.position + hitNormal * (wanderRadius * 0.5f);
                return true;
            }

            // Check if near the edge of the wandering area
            var distanceFromCenter = Vector3.Distance(transform.position, islandCenter.position);
            if (!(distanceFromCenter > wanderRadius - 2f)) return false;
            // Calculate normal based on direction to center
            var directionToCenter = (islandCenter.position - transform.position).normalized;
            avoidanceTarget = transform.position + directionToCenter * (wanderRadius * 0.5f);
            return true;

        }

        // Set a new random target within the wander radius
        private void SetNewTarget()
        {
            var randomAngle = Random.Range(0f, 360f);
            var offset = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)) * Random.Range(5f, wanderRadius - 5f);
            currentTarget = islandCenter.position + offset;
        }

        private void OnDrawGizmos()
        {
            if (!islandCenter) return;

            // Draw the wander area
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(islandCenter.position, wanderRadius);

            // Draw the current target
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(currentTarget, 1f);

            // Draw rays for obstacle detection
            Gizmos.color = Color.yellow;
            Vector3[] directions = {
                transform.forward,
                Quaternion.Euler(0, -25, 0) * transform.forward,
                Quaternion.Euler(0, 25, 0) * transform.forward
            };
            foreach (var dir in directions)
            {
                Gizmos.DrawRay(transform.position, dir * rayDistance);
            }
        }
    }
}
