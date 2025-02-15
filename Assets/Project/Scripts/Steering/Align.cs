using System;
using UnityEngine;

namespace Project.Scripts.Steering
{
    public class Align: Base
    {
        [SerializeField] private float targetAngularSpeed = 5f; 
        [SerializeField] private  float alignRadius = 5f;        
        [SerializeField] private float slowRadius = 15f;        
        
        private Controller controller;
        private void Start()
        {
            controller = GetComponent<Controller>();
        }

        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();
            
            var movementDirection = controller.GetVelocity();
        
            if (movementDirection.magnitude < 0.1f) return result;

            var targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            var angleDifference = Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle);
            var absAngleDiff = Mathf.Abs(angleDifference);

            if (absAngleDiff < alignRadius)
            {
                result.angular = 0; 
            }
            else
            {
                var rotationSpeed = targetAngularSpeed;
                if (absAngleDiff < slowRadius)
                {
                    rotationSpeed *= absAngleDiff / slowRadius;
                }

                result.angular = Mathf.Sign(angleDifference) * rotationSpeed;
            }

            return result;
        }
    }
}