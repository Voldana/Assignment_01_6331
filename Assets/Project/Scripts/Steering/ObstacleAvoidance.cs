using UnityEngine;

namespace Project.Scripts.Steering
{
    public class ObstacleAvoidance : Base
    {
        [SerializeField] private float maxAvoidanceForce = 100f;
        [SerializeField] private float detectionDistance = 10f;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private float rayOffset = 2f; 

        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();
            var forwardRay = transform.forward;
            var leftRay = Quaternion.Euler(0, -30, 0) * forwardRay;
            var rightRay = Quaternion.Euler(0, 30, 0) * forwardRay;

            // 🔹 Use normal of the obstacle surface to calculate the new direction
            if (!CheckObstacle(forwardRay, out var avoidanceForce) &&
                !CheckObstacle(leftRay, out avoidanceForce) &&
                !CheckObstacle(rightRay, out avoidanceForce)) return result;
            Debug.Log(avoidanceForce);
            result.linear = avoidanceForce;

            return result;
        }

        private bool CheckObstacle(Vector3 direction, out Vector3 avoidanceForce)
        {
            avoidanceForce = Vector3.zero;
            if (!Physics.Raycast(transform.position, direction, out var hit, detectionDistance, obstacleMask))
                return false;
            // 🔹 Instead of just moving away, use obstacle surface normal to find correct redirection
            var normal = hit.normal; // ✅ Get the surface normal of the obstacle
            avoidanceForce = normal * maxAvoidanceForce; // ✅ Move ship in the direction of the normal

            return true;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, transform.forward * detectionDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -30, 0) * transform.forward * detectionDistance);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 30, 0) * transform.forward * detectionDistance);
        }
    }
}