using UnityEngine;

namespace Project.Scripts.Steering
{
    public class Wander : Base
    {
        [SerializeField] private float wanderRadius = 5f;
        [SerializeField] private float wanderJitter = 2f;

        private Vector3 wanderTarget;
        private void Start()
        {
            wanderTarget = target.position;
        }

        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();
            if (!isActive) return result;
            wanderTarget += Random.insideUnitSphere * wanderJitter;
            wanderTarget = wanderTarget.normalized * wanderRadius;

            var worldTarget = transform.position + wanderTarget;
            result.linear = (worldTarget - transform.position).normalized * maxAcceleration;

            return result;
        }
    }
}