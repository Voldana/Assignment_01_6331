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
            SteeringOutput result = new SteeringOutput();

            // 1. Ensure the ship is moving before adjusting rotation
            if (GetComponent<Rigidbody>().linearVelocity.magnitude < 0.1f)
                return result; // Do nothing if not moving

            // 2. Compute the target orientation using the velocity
            Vector3 movementDirection = GetComponent<Rigidbody>().linearVelocity.normalized;
            float targetOrientation = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;

            // 3. Compute the rotation difference
            float rotationDifference = Mathf.DeltaAngle(transform.eulerAngles.y, targetOrientation);
            float absRotationDiff = Mathf.Abs(rotationDifference);

            // 4. Apply smooth rotation logic
            if (absRotationDiff < alignRadius)
            {
                result.angular = 0; // Stop rotating when aligned
            }
            else
            {
                float rotationSpeed = targetAngularSpeed;
                if (absRotationDiff < slowRadius)
                {
                    rotationSpeed *= absRotationDiff / slowRadius; // Reduce speed near target rotation
                }

                result.angular = Mathf.Sign(rotationDifference) * rotationSpeed;
            }

            return result;
        }
    }
}