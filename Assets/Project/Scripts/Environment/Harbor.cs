using System;
using Project.Scripts.Ships;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Environment
{
    public class Harbor : MonoBehaviour
    {
        [Inject] private SignalBus signalBus;

        [SerializeField] private Company.CompanyName company;
        [SerializeField] private Transform dockingPosition;
        [SerializeField] private float destructionRange;
        [SerializeField] private LayerMask pirateLayer;

        private bool isFull;

        public bool IsFull()
        {
            return isFull;
        }

        public void SetFull(bool status)
        {
            isFull = status;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((pirateLayer & (1 << other.gameObject.layer)) == 0) return;
            signalBus.Fire(new GameEvents.OnPirateDestroy() { pirate = other.gameObject });
            signalBus.Fire(new GameEvents.OnScoreChange{company = company, score = 25});
        }

        public Transform GetDockingPosition()
        {
            return dockingPosition;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Gizmos.DrawSphere(transform.position, destructionRange);
        }
    }
}