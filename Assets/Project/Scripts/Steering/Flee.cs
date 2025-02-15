using UnityEngine;

namespace Project.Scripts.Steering
{
    public class Flee : Base
    {
        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();
            if (!target) return result;

            result.linear = (transform.position - target.position).normalized * maxAcceleration;
            return result;
        }
    }
}