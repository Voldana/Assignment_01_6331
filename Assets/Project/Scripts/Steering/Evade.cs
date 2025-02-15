namespace Project.Scripts.Steering
{
    public class Evade: Base
    {
        public float predictionTime = 2f;

        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();
            if (!target) return result;

            var threatVelocity = target.GetComponent<Controller>().GetVelocity();
            var futurePosition = target.position + threatVelocity * predictionTime;

            result.linear = (transform.position - futurePosition).normalized * maxAcceleration;
            return result;
        }
    }
}