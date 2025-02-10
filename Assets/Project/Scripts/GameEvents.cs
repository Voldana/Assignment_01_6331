using UnityEngine;

namespace Project.Scripts
{
    public class GameEvents
    {
        public struct OnGameEnd
        {
            public string reason;
        }

        public struct OnPirateDestroy
        {
            public GameObject pirate;
        }
    }
}