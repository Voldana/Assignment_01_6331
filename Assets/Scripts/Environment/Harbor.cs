using Ships;
using UnityEngine;

namespace Environment
{
    public class Harbor : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("TradeShip")) return;
            var ship = other.GetComponent<Trade>();
            if (ship != null)
            {
                StartCoroutine(HandleArrival(ship));
            }
        }

        private System.Collections.IEnumerator HandleArrival(Trade ship)
        {
            yield return new WaitForSeconds(2); // Unloading/loading time
            FindObjectOfType<Manager>().ShipReachedHarbor(ship);
        }
    }
}