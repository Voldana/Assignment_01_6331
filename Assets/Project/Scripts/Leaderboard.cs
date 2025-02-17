using System;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Ships;
using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Scripts
{
    public class Leaderboard : MonoBehaviour
    {
        [Inject] private SignalBus signalBus;

        [SerializeField] private List<CompanyData> companyDatas;


        private void Start()
        {
            Subscribe();
            SetTextData();
        }

        private void SetTextData()
        {
            foreach (var data in companyDatas)
            {
                data.scoreText.color = Company.GetColor(data.company);
                data.scoreText.text = data.company.ToString() + " Company: " + data.score;
            }
        }

        private void Subscribe()
        {
            signalBus.Subscribe<GameEvents.OnScoreChange>(ChangeScore);
        }

        private void ChangeScore(GameEvents.OnScoreChange signal) 
        {
            var company = companyDatas.Find(data => data.company == signal.company);
            company.score += signal.score;
            company.scoreText.text = company.company + " Company: " + company.score;
        }

        public int GetScore(Company.CompanyName company)
        {
            return companyDatas.Find(data => data.company == company).score;
        }

        public KeyValuePair<Company.CompanyName, int> GetWinner()
        {
            var topScore = new KeyValuePair<Company.CompanyName, int>(Company.CompanyName.Blue, 0);
            /*foreach (var entry in scores.Where(entry => entry.Value > topScore.Value))
                topScore = entry;*/

            return topScore;
        }
    }

    [Serializable]
    public class CompanyData
    {
        public Company.CompanyName company;
        public TMP_Text scoreText;
        public int score;
    }
}