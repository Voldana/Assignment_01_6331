using UnityEngine;

namespace Project.Scripts.Steering
{
    public class NewWander : Base
    {
        [SerializeField] private Transform islandCenter; // Center of the island
        [SerializeField] private float wanderRadius = 30f; // Max distance from island
        [SerializeField] private float wanderSpeed = 5f; // Speed of wandering
        [SerializeField] private float turnSpeed = 2f; // Smooth turning
        [SerializeField] private float targetChangeThreshold = 3f; // Distance threshold to change target
        [SerializeField] private float edgeAvoidanceDistance = 5f; // Distance from edge to start turning inward

        private Vector3 currentTarget;

        private void Start()
        {
            SetNewTarget();
        }

        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();
            if (!islandCenter) return result;
            
            var directionToTarget = (currentTarget - transform.position).normalized;
            var desiredRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * turnSpeed);
            result.linear = transform.forward * wanderSpeed;
            if (Vector3.Distance(transform.position, currentTarget) < targetChangeThreshold)
                SetNewTarget();
            
            var distanceFromCenter = Vector3.Distance(transform.position, islandCenter.position);
            if (!(distanceFromCenter > wanderRadius - edgeAvoidanceDistance)) return result;
            var directionToCenter = (islandCenter.position - transform.position).normalized;
            result.linear += directionToCenter * wanderSpeed;

            return result;
        }
        
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
        }
    }
}
