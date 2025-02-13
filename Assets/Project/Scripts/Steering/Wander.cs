using UnityEngine;

namespace Project.Scripts.Steering
{
    public class Wander : Base
    {
        [SerializeField] private Transform islandCenter; 
        [SerializeField] private float wanderRadius = 30f; 
        [SerializeField] private float wanderSpeed = 5f; 
        [SerializeField] private float turnSpeed = 2f; 
        [SerializeField] private float targetChangeThreshold = 3f; 
        [SerializeField] private float edgeAvoidanceDistance = 5f; 

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

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(islandCenter.position, wanderRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(currentTarget, 1f);
        }
    }
}
