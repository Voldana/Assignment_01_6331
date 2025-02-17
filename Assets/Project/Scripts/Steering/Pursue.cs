using System;
using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.Steering
{
    public class Pursue : Base
    {
        [SerializeField] private float predictionTime = 2f;

        private PlayerController playerController;
        private Controller targetController;

        public override SteeringOutput GetSteering()
        {
            var result = new SteeringOutput();
            if (!target)
            {
                ClearTargets();
                return result;
            }
            
            var targetVelocity = GetTargetSpeed();
            var futurePosition = target.position + targetVelocity * predictionTime;

            result.linear = (futurePosition - transform.position).normalized * maxAcceleration;
            return result;
        }
        
        private Vector3 GetTargetSpeed()
        {
            if (!target) return Vector3.zero;
            if (targetController is null && playerController is null)
            {
                if (target.CompareTag("Player"))
                    playerController = target.GetComponent<PlayerController>();
                else if(target.CompareTag("Trading"))
                    targetController = target.GetComponent<Controller>();
            }
            if (playerController)
                return playerController.GetVelocity();
            
            if(targetController)
                return targetController.GetVelocity();
            
            return Vector3.zero;
        }

        public void ClearTargets()
        {
            targetController = null;
            playerController = null;
        }
        
    }
}