using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Project.Scripts.Ships
{
    public class Capture : MonoBehaviour
    {
        [Inject] private SignalBus signalBus;
        private int levelNumber;
        private void Start()
        {
            int.TryParse(SceneManager.GetActiveScene().name, out  levelNumber);
            if(levelNumber > 2) return;
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Trading")) return;
            var trade = other.gameObject.GetComponent<Trade>();
            if(!trade) return;
            trade.Death();
        }
    }
}
