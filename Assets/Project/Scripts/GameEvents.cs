using Project.Scripts.Ships;
using UnityEngine;

namespace Project.Scripts
{
    public class GameEvents
    {
        public struct OnGameEnd
        {
            public string reason;
        }

        public struct OnCollision
        {
            public GameObject collided;
        }
        public struct OnPirateDestroy
        {
            public GameObject pirate;
        }
        
        public struct OnTradeShipDestroy
        {
            public Company.CompanyName company;
        }
        
        public struct OnScoreChange
        {
            public Company.CompanyName company;
            public int score;
        }
    }
}