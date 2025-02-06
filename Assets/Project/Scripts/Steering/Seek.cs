using UnityEngine;

namespace Project.Scripts.Steering
{
    public class Seek : Base
    {
        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();
            if (target == null) return result;

            result.linear = (target.position - transform.position).normalized * maxAcceleration;
            return result;
        }
    }
}