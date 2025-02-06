using UnityEngine;

namespace Project.Scripts.Steering
{
    public class Arrive : Base
    {
        [SerializeField] private float arrivalRadius = 3f;
        [SerializeField] private float slowRadius = 8f;

        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();
            if (target == null) return result;

            var direction = target.position - transform.position;
            var distance = direction.magnitude;

            if (distance < arrivalRadius)
            {
                result.linear = Vector3.zero;
                return result;
            }

            var targetSpeed = maxAcceleration * (distance / slowRadius);
            targetSpeed = Mathf.Min(targetSpeed, maxAcceleration);

            result.linear = direction.normalized * targetSpeed;
            return result;
        }
    }
}