using UnityEngine;

namespace Project.Scripts.Steering
{
    public class ObstacleAvoidance : Base
    {
        [SerializeField] private float maxAvoidanceForce = 100f;
        [SerializeField] private float detectionDistance = 10f;
        [SerializeField] private LayerMask obstacleMask;
        
        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();
            var forwardRay = transform.forward;
            var leftRay = Quaternion.Euler(0, -25, 0) * forwardRay;
            var rightRay = Quaternion.Euler(0, 25, 0) * forwardRay;

            
            if (CheckObstacle(forwardRay, out var avoidanceForce) ||
                CheckObstacle(leftRay, out avoidanceForce) ||
                CheckObstacle(rightRay, out avoidanceForce))
            {
                result.linear = avoidanceForce;
            }

            return result;
        }

        private bool CheckObstacle(Vector3 direction, out Vector3 avoidanceForce)
        {
            avoidanceForce = Vector3.zero;
            if (!Physics.Raycast(transform.position, direction, out var hit, detectionDistance, obstacleMask))
                return false;
            var normal = hit.normal;
            avoidanceForce = normal * maxAvoidanceForce;  
            return true;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, transform.forward * detectionDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -25, 0) * transform.forward * detectionDistance);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 25, 0) * transform.forward * detectionDistance);
        }
    }
}
