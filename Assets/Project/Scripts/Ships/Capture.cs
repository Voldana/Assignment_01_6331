using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Project.Scripts.Ships
{
    public class Capture : MonoBehaviour
    {
        [Inject] private SignalBus signalBus;
        private void Start()
        {
            int.TryParse(SceneManager.GetActiveScene().name, out var levelNumber);
            if(levelNumber > 2) return;
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Trading"))
                signalBus.Fire(new GameEvents.OnGameEnd(){reason = "Trade ship was captured"});
        }
    }
}
