using System.Collections;
using Project.Scripts.Ships;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Scripts
{
    public class Manager : MonoBehaviour
    {


        [SerializeField] private Transform[] harbors;
        [SerializeField] public Trade trade;
        private int currentHarborIndex = 0;
        public TMP_Text scoreText;
        public TMP_Text timerText;
    
        private int score = 0;
        private float gameTime = 300f; // 5 minutes

        void Start()
        {
            // trade.SetTarget(harbors[currentHarborIndex].position);
            UpdateScore(0);
            StartCoroutine(GameTimer());
        }

        public void ShipReachedHarbor(Trade ship)
        {
            score += 10; // Increase score when reaching harbor
            UpdateScore(score);
            currentHarborIndex = (currentHarborIndex + 1) % harbors.Length;
            // ship.SetTarget(harbors[currentHarborIndex].position);
        }

        private IEnumerator GameTimer()
        {
            while (gameTime > 0)
            {
                gameTime -= Time.deltaTime;
                timerText.text = "Time: " + Mathf.Ceil(gameTime);
                yield return null;
            }
            EndGame();
        }

        private void UpdateScore(int points)
        {
            scoreText.text = "Score: " + points;
        }

        public void EndGame()
        {
            Debug.Log("Game Over!");
            Time.timeScale = 0;
        }
    }
}


