using System;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Environment
{
    public class Crate : MonoBehaviour
    {
        [Inject] private SignalBus signalBus;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Pirate") || other.gameObject.CompareTag("Trading") ||
                other.gameObject.CompareTag("Player"))
            {
                signalBus.Fire(new GameEvents.OnCollision { collided = other.gameObject });
            }
        }
    }
}