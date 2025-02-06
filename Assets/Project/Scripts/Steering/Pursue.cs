using UnityEngine;

namespace Project.Scripts.Steering
{
    public class Pursue : Base
    {
        [SerializeField] private float predictionTime = 2f;


        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();
            if (target == null) return result;

            var targetVelocity = target.GetComponent<Controller>().GetVelocity();
            var futurePosition = target.position + targetVelocity * predictionTime;

            result.linear = (futurePosition - transform.position).normalized * maxAcceleration;
            return result;
        }
    }
}