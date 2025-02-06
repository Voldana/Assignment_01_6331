using UnityEngine;

namespace Project.Scripts.Ships
{
    public class Pirate : MonoBehaviour
    {
        [SerializeField] private float fieldOfViewAngle = 120f;
        [SerializeField] private float detectionRange = 15f;

        public Transform islandCenter;
        private Trade targetTradeShip;
        private bool isChasing = false;

        /*private void Update()
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
            if (!(Vector3.Distance(transform.position, targetPosition) < 2f)) return;
            float randomAngle = Random.Range(0, 360);
            Vector3 offset = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0) * 10;
            SetTarget(islandCenter.position + offset);
        }*/

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
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            var leftLimit = Quaternion.Euler(0, 0, fieldOfViewAngle / 2) * transform.up * detectionRange;
            var rightLimit = Quaternion.Euler(0, 0, -fieldOfViewAngle / 2) * transform.up * detectionRange;

            Gizmos.DrawLine(transform.position, transform.position + leftLimit);
            Gizmos.DrawLine(transform.position, transform.position + rightLimit);
        }
    }
}