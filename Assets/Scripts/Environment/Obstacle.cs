using UnityEngine;

namespace Environment
{
    public class Obstacle : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("TradeShip") || other.CompareTag("PirateShip"))
            {
                Debug.Log(other.name + " collided with an obstacle!");
            }
        }
    }
}