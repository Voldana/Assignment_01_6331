using System;
using UnityEngine;

namespace Project.Scripts.Ships
{
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField] private Pirate pirate;

        private GameObject currentTarget;
        private Trade tradeShip;
        private bool isPursuing;

        private void OnTriggerEnter(Collider other)
        {
            if (!isPursuing && other.CompareTag("Trading"))
                TryAndPursue(other.gameObject);
            if(!isPursuing && other.CompareTag("Player"))
                PursuePlayer(other.gameObject);
        }

        private void TryAndPursue(GameObject target)
        {
            if (!target.TryGetComponent<Trade>(out var trading)) return;
            isPursuing = true;
            tradeShip = trading;
            currentTarget = trading.gameObject;
            trading.StartFleeing(pirate);
            pirate.StartChasing(target);
            
        }

        private void PursuePlayer(GameObject target)
        {
            isPursuing = true;
            currentTarget = target;
            pirate.StartChasing(target);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!isPursuing || !other.gameObject.Equals(currentTarget)) return;
            StopChasing();
        }
        
        public void StopChasing()
        {
            isPursuing = false;
            pirate.StopChasing();
            if(!tradeShip) return;
            tradeShip.StopFleeing();
        }
    }
}