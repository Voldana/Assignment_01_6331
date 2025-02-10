using UnityEngine;

namespace Project.Scripts.Steering
{
    public class Wander : Base
    {
        [SerializeField] private Transform islandCenter; // The center of the wandering area
        [SerializeField] private float wanderRadius = 20f; // Maximum distance from the island
        [SerializeField] private float wanderSpeed = 5f; // Movement speed
        [SerializeField] private float turnSpeed = 2f; // Controls smooth turning
        [SerializeField] private float edgeAvoidanceForce = 50f; // Strength of avoidance near the edge
        [SerializeField] private float targetChangeDistance = 5f; // Distance before selecting a new target

        private Vector3 currentTarget;

        private void Start()
        {
            if (!islandCenter)
            {
                Debug.LogError("IslandCenter is not assigned in WanderAroundIsland!");
                return;
            }

            SetNewTarget(); // Set the first random target
        }

        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();
            if (!islandCenter) return result;

            // 1️⃣ Move toward the wander target
            var directionToTarget = (currentTarget - transform.position).normalized;

            // 2️⃣ Rotate smoothly toward the target
            var desiredRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * turnSpeed);

            // 3️⃣ Move forward
            result.linear = transform.forward * wanderSpeed;

            // 4️⃣ If close to target, pick a new one
            if (Vector3.Distance(transform.position, currentTarget) < targetChangeDistance)
            {
                SetNewTarget();
            }

            // 5️⃣ Edge Avoidance: Steer inward if close to the wander radius
            var distanceFromCenter = Vector3.Distance(transform.position, islandCenter.position);
            if (distanceFromCenter >= wanderRadius * 0.9f)
            {
                var directionToCenter = (islandCenter.position - transform.position).normalized;
                result.linear += directionToCenter * edgeAvoidanceForce; // Push the ship back inward
            }

            return result;
        }

        private void SetNewTarget()
        {
            // 6️⃣ Pick a new random target inside the wander area
            var randomAngle = Random.Range(0f, 360f);
            var offset = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)) * Random.Range(5f, wanderRadius * 0.8f);
            currentTarget = islandCenter.position + offset;
        }

        private void OnDrawGizmos()
        {
            if (!islandCenter) return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(islandCenter.position, wanderRadius); // Shows the wander area

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(currentTarget, 1f); // Shows the current wandering target
        }
    }
}
