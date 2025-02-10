using UnityEngine;

namespace Project.Scripts.Steering
{
    public class Wander : Base
    {
        [SerializeField] private Transform islandCenter; // The island to circle around
        [SerializeField] private float wanderRadius = 20f; // Distance from island
        [SerializeField] private float wanderSpeed = 5f; // Base movement speed
        [SerializeField] private float turnSpeed = 2f; // Smooth rotation
        [SerializeField] private float wanderJitter = 3f; // Small random movement changes
        [SerializeField] private float boundaryAvoidanceStrength = 5f; // Strength of avoidance when near boundary

        private Vector3 wanderTarget;
        private bool avoidingBoundary = false; // Flag to prevent infinite turning

        private void Start()
        {
            if (!islandCenter)
            {
                Debug.LogError("IslandCenter is not assigned to WanderAroundIsland script!");
                return;
            }

            // Start at a random position around the island
            SetNewTarget();
        }

        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();
            if (!islandCenter) return result;

            // 1️⃣ Detect if the ship is near the boundary
            var distanceFromIsland = Vector3.Distance(transform.position, islandCenter.position);
            if (distanceFromIsland >= wanderRadius * 0.9f) // Near boundary
            {
                avoidingBoundary = true;
            }
            else
            {
                avoidingBoundary = false;
            }

            // 2️⃣ If near boundary, smoothly steer back toward the center
            if (avoidingBoundary)
            {
                var directionToCenter = (islandCenter.position - transform.position).normalized;
                var avoidanceForce = directionToCenter * boundaryAvoidanceStrength;
                result.linear = avoidanceForce;
            }
            else
            {
                // 3️⃣ Normal wandering behavior
                var directionToTarget = (wanderTarget - transform.position).normalized;

                // Smoothly rotate towards target
                var desiredRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * turnSpeed);

                // Apply movement forward
                result.linear = transform.forward * wanderSpeed;

                // Slightly adjust wander target to avoid straight lines
                ApplyWanderJitter();

                // 4️⃣ If close to the target, select a new one
                if (Vector3.Distance(transform.position, wanderTarget) < 5f)
                {
                    SetNewTarget();
                }
            }

            return result;
        }

        private void SetNewTarget()
        {
            var randomAngle = Random.Range(0f, 360f);
            var offset = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)) * wanderRadius;
            wanderTarget = islandCenter.position + offset;
        }

        private void ApplyWanderJitter()
        {
            var jitter = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * wanderJitter;
            wanderTarget += jitter;
            wanderTarget = islandCenter.position + (wanderTarget - islandCenter.position).normalized * wanderRadius;
        }

        private void OnDrawGizmos()
        {
            if (!islandCenter) return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(islandCenter.position, wanderRadius); // Shows the orbiting area

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(wanderTarget, 1f); // Shows the current wandering target
        }
    }
}
