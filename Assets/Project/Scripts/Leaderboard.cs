using System;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Ships;
using UnityEngine;
using Zenject;

namespace Project.Scripts
{
    public class Leaderboard : MonoBehaviour
    {
        [Inject] private SignalBus signalBus;

        private Dictionary<Company.CompanyName, int> scores = new Dictionary<Company.CompanyName, int>();

        private void Start()
        {
            foreach (Company.CompanyName company in Enum.GetValues(typeof(Company.CompanyName)))
                scores[company] = 0;

            Subscribe();
        }

        private void Subscribe()
        {
            signalBus.Subscribe<GameEvents.OnScoreChange>(ChangeScore);
        }

        private void ChangeScore(GameEvents.OnScoreChange signal)
        {
            if (scores.ContainsKey(signal.company))
                scores[signal.company] += signal.score;
        }

        public int GetScore(Company.CompanyName company)
        {
            return scores.GetValueOrDefault(company, 0);
        }

        public KeyValuePair<Company.CompanyName, int> GetWinner()
        {
            var topScore = new KeyValuePair<Company.CompanyName, int>(Company.CompanyName.Blue, 0);
            foreach (var entry in scores.Where(entry => entry.Value > topScore.Value))
                topScore = entry;
            
            return topScore;
        }
    }
}