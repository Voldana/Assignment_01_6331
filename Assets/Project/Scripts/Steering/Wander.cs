using UnityEngine;

namespace Project.Scripts.Steering
{
    public class Wander : Base
    {
        [SerializeField] private Transform islandCenter;
        [SerializeField] private float wanderRadius = 30f;
        [SerializeField] private float wanderSpeed = 5f;
        [SerializeField] private float turnSpeed = 3f;
        [SerializeField] private float targetChangeThreshold = 3f;
        [SerializeField] private float rayDistance = 5f;
        [SerializeField] private LayerMask obstacleMask;

        private Vector3 currentTarget;
        private Vector3 lastValidTarget;

        private void Start()
        {
            SetNewTarget();
            lastValidTarget = currentTarget;
        }

        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();

            if (DetectObstacleOrEdge(out var avoidanceTarget))
            {
                if (avoidanceTarget != Vector3.zero && avoidanceTarget != currentTarget)
                {
                    currentTarget = avoidanceTarget;
                    lastValidTarget = currentTarget;
                }
            }
            else
            {
                currentTarget = lastValidTarget;
            }

            var directionToTarget = (currentTarget - transform.position).normalized;
            directionToTarget.y = 0;

            if (directionToTarget == Vector3.zero)
                directionToTarget = transform.forward;

            var desiredRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * turnSpeed);

            result.linear = transform.forward * wanderSpeed;

            if (Vector3.Distance(transform.position, currentTarget) < targetChangeThreshold)
                SetNewTarget();

            return result;
        }

        private bool DetectObstacleOrEdge(out Vector3 avoidanceTarget)
        {
            avoidanceTarget = Vector3.zero;
            Vector3[] directions = {
                transform.forward,
                Quaternion.Euler(0, -25, 0) * transform.forward,
                Quaternion.Euler(0, 25, 0) * transform.forward
            };

            foreach (var direction in directions)
            {
                if (!Physics.Raycast(transform.position, direction, out RaycastHit hit, rayDistance,
                        obstacleMask)) continue;
                var hitNormal = hit.normal;
                hitNormal.y = 0;
                avoidanceTarget = transform.position + hitNormal * (wanderRadius * 0.5f);
                return true;
            }

            var distanceFromCenter = Vector3.Distance(transform.position, islandCenter.position);
            if (!(distanceFromCenter > wanderRadius - 2f)) return false;
            var directionToCenter = (islandCenter.position - transform.position).normalized;
            directionToCenter.y = 0;
            avoidanceTarget = transform.position + directionToCenter * (wanderRadius * 0.5f);
            return true;

        }

        private void SetNewTarget()
        {
            var randomAngle = Random.Range(0f, 360f);
            var offset = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)) * Random.Range(5f, wanderRadius - 5f);
            currentTarget = islandCenter.position + offset;
            lastValidTarget = currentTarget;
        }

        private void OnDrawGizmos()
        {
            if (!islandCenter) return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(islandCenter.position, wanderRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(currentTarget, 1f);

            Gizmos.color = Color.yellow;
            Vector3[] directions = {
                transform.forward,
                Quaternion.Euler(0, -25, 0) * transform.forward,
                Quaternion.Euler(0, 25, 0) * transform.forward
            };
            foreach (var dir in directions)
                Gizmos.DrawRay(transform.position, dir * rayDistance);
        }
    }
}
