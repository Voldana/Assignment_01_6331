using UnityEngine;

namespace Project.Scripts.Ships
{
    public class Pirate : MonoBehaviour
    {
        [SerializeField] private float fieldOfViewAngle = 120f;
        [SerializeField] private float detectionRange = 15f;

        /*void Start()
        {
            gameObject.AddComponent<SteeringController>();
        
            Wander wander = gameObject.AddComponent<Wander>();
            wander.island = island;

            Pursue pursue = gameObject.AddComponent<Pursue>();
            pursue.target = tradeShip;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("TradeShip"))
            {
                targetTradeShip = other.GetComponent<Trade>();
                isChasing = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("TradeShip"))
            {
                isChasing = false;
                targetTradeShip = null;
            }
        }*/
    }
}