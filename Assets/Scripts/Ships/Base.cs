using UnityEngine;

namespace Ships
{
    public class Base : MonoBehaviour
    {
        public float maxSpeed = 5f;
        public float acceleration = 2f;
        public float rotationSpeed = 2f;

        protected Vector3 velocity;
        protected Vector3 targetPosition;

        public virtual void SetTarget(Vector3 target)
        {
            targetPosition = target;
        }

        protected void Move()
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            velocity += direction * acceleration * Time.deltaTime;
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
            transform.position += velocity * Time.deltaTime;
            transform.up = velocity.normalized; // Face movement direction
        }
    }
}