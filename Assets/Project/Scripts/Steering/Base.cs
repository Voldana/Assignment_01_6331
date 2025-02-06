using UnityEngine;

namespace Project.Scripts.Steering
{
    public abstract class Base : MonoBehaviour
    {
        [SerializeField] protected float maxAcceleration = 2f;

        protected Transform target;

        public abstract SteeringOutput GetSteering();

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }

    public struct SteeringOutput
    {
        public Vector3 linear;
        public float angular;
    }
}