using System;
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
        [SerializeField] private Leaderboard leaderboard;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text clock;
        
        [Inject] private LevelMenu.Factory levelFactory;
        [Inject] private LoseMenu.Factory loseFactory;
        [Inject] private SignalBus signalBus;
        

        private Timer timer;

        private void Start()
        {
            Time.timeScale = 1;
            DOTween.timeScale = 1;
            SubscribeSignals();
            SetTimer();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
                OnPauseClick();
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
            loseFactory.Create("Game Ended!",leaderboard).transform.SetParent(transform.parent, false);
        }

        private void SetTimer()
        {
            UpdateClock(60);
            timer = new Timer(60, UpdateClock, OnTimerEnd);
            timer.Start();
        }

        public void OnPauseClick()
        {
            levelFactory.Create().transform.SetParent(transform.parent, false);
        }

        private void UpdateClock(int newTime)
        {
            clock.text = $"{newTime / 60}:{newTime % 60:D2}";
        }

        private void OnTimerEnd()
        {
            timer.Stop();
            ShowLoseScreen();
        }
    }
}