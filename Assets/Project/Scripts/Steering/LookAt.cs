using UnityEngine;

namespace Project.Scripts.Steering
{
    public class LookAt: Base
    {
        public float targetAngularSpeed = 5f; // Max rotation speed
        public float alignRadius = 5f; // When to start slowing rotation
        public float slowRadius = 15f; // Smooth rotation transition

        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();

            if (GetComponent<Rigidbody>().linearVelocity.magnitude < 0.1f)
                return result;

            var movementDirection = GetComponent<Rigidbody>().linearVelocity.normalized;
            var targetOrientation = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;

            var rotationDifference = Mathf.DeltaAngle(transform.eulerAngles.y, targetOrientation);
            var absRotationDiff = Mathf.Abs(rotationDifference);

            if (absRotationDiff < alignRadius)
            {
                result.angular = 0;
            }
            else
            {
                var rotationSpeed = targetAngularSpeed;
                if (absRotationDiff < slowRadius)
                {
                    rotationSpeed *= absRotationDiff / slowRadius; 
                }

                result.angular = Mathf.Sign(rotationDifference) * rotationSpeed;
            }

            return result;
        }
    }
}