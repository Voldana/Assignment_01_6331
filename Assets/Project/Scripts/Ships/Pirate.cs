using System;
using Project.Scripts.Steering;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Ships
{
    public class Pirate : MonoBehaviour
    {
        [Inject] private SignalBus signalBus;
        
        [SerializeField] private Pursue pursue;
        [SerializeField] private Wander wander;

        private void Start()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            signalBus.Subscribe<GameEvents.OnPirateDestroy>(SelfDestruct);
        }

        private void SelfDestruct(GameEvents.OnPirateDestroy signal)
        {
            if(!signal.pirate.Equals(gameObject)) return;
            Destroy(gameObject);
        }

        public void StartChasing(GameObject target)
        {
            wander.SetStatus(false);
            pursue.SetTarget(target.transform);
        }

        public void StopChasing()
        {
            wander.SetStatus(true);
            pursue.SetTarget(null);
        }
    }
}