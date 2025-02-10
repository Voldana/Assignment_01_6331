using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.Steering
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private float maxRotation = 20f;
        [SerializeField] private float maxSpeed = 50f;
        [SerializeField] private float drag = 0.1f; 

        private Vector3 velocity = Vector3.zero;
        private float rotation;

        private void Update()
        {
            var totalSteering = new SteeringOutput();

            var behaviors = GetComponents<Base>();
            foreach (var behavior in behaviors)
            {
                var steering = behavior.GetSteering();
                totalSteering.linear += steering.linear * behavior.GetWeight();
                totalSteering.angular += steering.angular * behavior.GetWeight();
            }

            velocity *= (1 - drag);
            rotation *= (1 - drag);

            velocity += totalSteering.linear * Time.deltaTime;
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

            rotation += totalSteering.angular * Time.deltaTime;
            rotation = Mathf.Clamp(rotation, -maxRotation, maxRotation);
            velocity.y = 0;
            transform.position += velocity * Time.deltaTime;
            
            if (!(velocity.magnitude > 0.1f)) return;
            var targetRotation = Quaternion.LookRotation(velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.5f);
        }

        public Vector3 GetVelocity()
        {
            return velocity;
        }
    }
}

