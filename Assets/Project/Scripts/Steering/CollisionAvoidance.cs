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

            if (obstacles.Length == 0) return result;

            Transform closestObstacle = null;
            var closestTimeToCollision = float.MaxValue;
            var closestRelativeVelocity = Vector3.zero;

            foreach (var obstacle in obstacles)
            {
                if (obstacle.gameObject == gameObject) continue;

                var relativeVelocity = Vector3.zero;
                if (obstacle.TryGetComponent<Controller>(out var tarController))
                    relativeVelocity = tarController.GetVelocity() - controller.GetVelocity();
                

                var relativePosition = obstacle.transform.position - transform.position;
                float timeToCollision;

                if (relativeVelocity.sqrMagnitude > 0.01f)
                    timeToCollision = Vector3.Dot(relativePosition, relativeVelocity) / relativeVelocity.sqrMagnitude;
                
                else
                    continue; 
                

                if (!(timeToCollision >= 0) || !(timeToCollision < closestTimeToCollision)) continue;

                closestTimeToCollision = timeToCollision;
                closestObstacle = obstacle.transform;
                closestRelativeVelocity = relativeVelocity;
            }

            if (!closestObstacle) return result; 
            
            var avoidanceForce = -closestRelativeVelocity.normalized * maxAvoidanceForce;
            result.linear = avoidanceForce;
            return result;
        }
        
        private readonly Color detectionColor = new Color(1f, 0.5f, 0f, 0.2f);
        private readonly Color avoidanceForceColor = Color.red;
        private readonly Color obstacleLineColor = Color.yellow;
        private void OnDrawGizmos()
        {

            Gizmos.color = detectionColor;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            var obstacles = Physics.OverlapSphere(transform.position, detectionRadius, obstacleMask);

            foreach (var obstacle in obstacles)
            {

                Gizmos.color = obstacleLineColor;
                Gizmos.DrawLine(transform.position, obstacle.transform.position);
            }

            var steering = GetSteering();
            if (!(steering.linear.magnitude > 0.1f)) return;
            Gizmos.color = avoidanceForceColor;
            Gizmos.DrawLine(transform.position, transform.position + steering.linear.normalized * 3f); 
        }
    }
}