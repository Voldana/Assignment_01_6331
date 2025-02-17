using System;
using Project.Scripts.Steering;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Ships
{
    public class Pirate : MonoBehaviour
    {
        [Inject] private SignalBus signalBus;
        
        [SerializeField] private FieldOfView fieldOfView;
        [SerializeField] private Pursue pursue;
        [SerializeField] private Wander wander;

        private void Start()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            signalBus.Subscribe<GameEvents.OnPirateDestroy>(SelfDestruct);
            signalBus.Subscribe<GameEvents.OnCollision>(SelfDestruct);
        }

        private void SelfDestruct(GameEvents.OnPirateDestroy signal)
        {
            if(!signal.pirate.Equals(gameObject)) return;
            Unsubscribe();
            fieldOfView.StopChasing();
            Destroy(gameObject);
        }        
        private void SelfDestruct(GameEvents.OnCollision signal)
        {
            if(!signal.collided.Equals(gameObject)) return;
            Unsubscribe();
            fieldOfView.StopChasing();
            Destroy(gameObject);
        }

        public void StartChasing(GameObject target)
        {
            wander.SetStatus(false);
            pursue.SetTarget(target.transform);
        }

        private void Unsubscribe()
        {
            signalBus.TryUnsubscribe<GameEvents.OnPirateDestroy>(SelfDestruct);
            signalBus.TryUnsubscribe<GameEvents.OnCollision>(SelfDestruct);
        }

        public void StopChasing()
        {
            wander.SetStatus(true);
            pursue.SetTarget(null);
        }
    }
}