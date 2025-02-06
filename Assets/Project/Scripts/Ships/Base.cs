using UnityEngine;
using Zenject;

namespace Project.Scripts.Ships
{
    public class Base : MonoBehaviour
    {
        [Inject] protected SignalBus signalBus;
        
        [SerializeField] protected float rotationSpeed = 2f;
        [SerializeField] protected float acceleration = 2f;
        [SerializeField] protected float maxSpeed = 5f;

        protected Vector3 velocity;
        protected Vector3 targetPosition;

        public virtual void SetTarget(Vector3 target)
        {
            targetPosition = target;
        }

        protected void Move()
        {
            var direction = (targetPosition - transform.position).normalized;
            velocity += direction * acceleration * Time.deltaTime;
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
            transform.position += velocity * Time.deltaTime;
            transform.up = velocity.normalized; // Face movement direction
        }
    }
}