using UnityEngine;

namespace Ships
{
    public class Pirate : Base
    {
        public float detectionRange = 10f;
        public Transform islandCenter;
        private Trade targetTradeShip;
        private bool isChasing = false;

        void Update()
        {
            if (isChasing && targetTradeShip != null)
            {
                SetTarget(targetTradeShip.transform.position);
            }
            else
            {
                WanderAroundIsland();
            }
            Move();
        }

        private void WanderAroundIsland()
        {
            if (Vector3.Distance(transform.position, targetPosition) < 2f)
            {
                float randomAngle = Random.Range(0, 360);
                Vector3 offset = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0) * 10;
                SetTarget(islandCenter.position + offset);
            }
        }

        void OnTriggerEnter(Collider other)
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
        }
    }
}