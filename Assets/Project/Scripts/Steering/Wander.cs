using UnityEngine;

namespace Project.Scripts.Steering
{
    public class Wander : Base
    {
        [SerializeField] private Transform islandCenter; // The island to circle around
        [SerializeField] private float wanderRadius = 20f; // Distance from island
        [SerializeField] private float wanderSpeed = 5f; // Base movement speed
        [SerializeField] private float turnSpeed = 2f; // Smooth rotation
        [SerializeField] private float wanderJitter = 3f;  // Prevents excessive acceleration
        [SerializeField] private float edgeSlowdownDistance = 5f; // Distance from edge to start slowing

        private Vector3 wanderTarget;
        private bool adjustingForEdge = false;

        private void Start()
        {
            if (!islandCenter)
            {
                Debug.LogError("IslandCenter is not assigned to WanderAroundIsland script!");
                return;
            }

            SetNewTarget();
        }

        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();
            if (!islandCenter) return result;

            // 1️⃣ Check if near the boundary of the wander area
            var distanceFromIsland = Vector3.Distance(transform.position, islandCenter.position);
            if (distanceFromIsland >= wanderRadius - edgeSlowdownDistance)
            {
                adjustingForEdge = true;
            }
            else
            {
                adjustingForEdge = false;
            }

            // 2️⃣ If near the boundary, gradually steer back to the center
            if (adjustingForEdge)
            {
                var directionToCenter = (islandCenter.position - transform.position).normalized;
                result.linear = directionToCenter * (wanderSpeed / 2f); // Reduce speed near the edge
            }
            else
            {
                // 3️⃣ Normal wandering behavior inside the wander area
                var directionToTarget = (wanderTarget - transform.position).normalized;

                // Smoothly rotate towards target
                var desiredRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * turnSpeed);

                // Apply movement forward
                result.linear = transform.forward * wanderSpeed;

                // Adjust wander target slightly to prevent straight lines
                ApplyWanderJitter();

                // 4️⃣ If close to the target, pick a new one
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
