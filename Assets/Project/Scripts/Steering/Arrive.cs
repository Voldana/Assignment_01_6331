using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Project.Scripts.Steering
{
    public class Arrive : Base
    {
        [SerializeField] private float arrivalRadius = 3f;
        [SerializeField] private float slowRadius = 8f;

        private Action onArrive;
        private bool arrived;

        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();
            if (target == null) return result;

            var direction = target.position - transform.position;
            var distance = direction.magnitude;

            if (distance < arrivalRadius)
            {
                result.linear = Vector3.zero;
                OnArrive();
                return result;
            }

            arrived = false;
            var targetSpeed = maxAcceleration * (distance / slowRadius);
            targetSpeed = Mathf.Min(targetSpeed, maxAcceleration);

            result.linear = direction.normalized * targetSpeed;
            return result;
        }

        public void SetAction(Action action)
        {
            onArrive = action;
        }

        private void OnArrive()
        {
            if (arrived) return;
            arrived = true;
            onArrive?.Invoke();
        }
    }
}