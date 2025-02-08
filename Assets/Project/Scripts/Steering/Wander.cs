using UnityEngine;

namespace Project.Scripts.Steering
{
    public class Wander : Base
    {
        [SerializeField] private float wanderRadius = 5f;
        [SerializeField] private float wanderJitter = 2f;
        [SerializeField] private Transform wanderTarget;

        private Vector3 wander;
        private void Start()
        {
            wander = wanderTarget.position;
        }

        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();
            if (!isActive) return result;
            wander += Random.insideUnitSphere * wanderJitter;
            wander = wander.normalized * wanderRadius;

            var worldTarget = transform.position + wander;
            result.linear = (worldTarget - transform.position).normalized * maxAcceleration;

            return result;
        }
    }
}