using System;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Environment
{
    public class Harbor : MonoBehaviour
    {
        [Inject] private SignalBus signalBus;

        [SerializeField] private Transform dockingPosition;
        [SerializeField] private float destructionRange;
        [SerializeField] private LayerMask pirateLayer;

        private bool isFull;
        private int score;

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
            if (!other.gameObject.layer.Equals(pirateLayer)) return;
            signalBus.Fire(new GameEvents.OnPirateDestroy() { pirate = other.gameObject });
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