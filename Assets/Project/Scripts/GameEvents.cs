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
    }
}