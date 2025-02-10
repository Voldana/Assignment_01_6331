using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Scripts.UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text clock;

        [Inject] private LoseMenu.Factory loseFactory;
        [Inject] private SignalBus signalBus;

        private bool timeEnded;
        
        private Timer timer;

        private void Start()
        {
            Time.timeScale = 1;
            DOTween.timeScale = 1;
            SubscribeSignals();
            SetTimer();
        }

        private void SubscribeSignals()
        {
            signalBus.Subscribe<GameEvents.OnGameEnd>(GameEnded);
        }

        private void GameEnded(GameEvents.OnGameEnd signal)
        {
            ShowLoseScreen();
        }

        private void ShowLoseScreen()
        {
            timer.Stop();
            Time.timeScale = 0;
            DOTween.KillAll();
            DOVirtual.DelayedCall(1.5f, () =>
            {
                loseFactory.Create(new LossDetails()
                {
                    /*score = score,
                    reason = "",
                    level = levelSetting.level*/
                }).transform.SetParent(transform.parent, false);
            });
        }
        
        

        private void SetTimer()
        {
            UpdateClock(300);
            timer = new Timer(300, UpdateClock, OnTimerEnd);
            timer.Start();
        }

        private void UpdateClock(int newTime)
        {
            clock.text = $"{newTime / 60}:{newTime % 60:D2}";
        }

        private void OnTimerEnd()
        {
            timeEnded = true;
            timer.Stop();
            ShowLoseScreen();
        }
    }
}