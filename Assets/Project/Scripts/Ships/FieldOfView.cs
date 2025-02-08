using System;
using UnityEngine;

namespace Project.Scripts.Ships
{
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField] private Pirate pirate;

        private Trade currentTarget;
        private bool isPursuing;

        private void OnTriggerEnter(Collider other)
        {
            if (!isPursuing && other.CompareTag("Trading")){}
                TryAndPursue(other.gameObject);
        }

        private void TryAndPursue(GameObject target)
        {
            if (!target.TryGetComponent<Trade>(out var trading)) return;
            isPursuing = true;
            currentTarget = trading;
            trading.StartFleeing(pirate);
            pirate.StartChasing(target);
            
        }

        private void OnTriggerExit(Collider other)
        {
            if (!isPursuing || !other.gameObject.Equals(currentTarget.gameObject)) return;
            isPursuing = false;
            pirate.StopChasing();
            currentTarget.StopFleeing();
        }
    }
}