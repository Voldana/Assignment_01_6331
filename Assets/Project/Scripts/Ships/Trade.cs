using System.Collections.Generic;
using DG.Tweening;
using Project.Scripts.Environment;
using Project.Scripts.Steering;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Random = UnityEngine.Random;

namespace Project.Scripts.Ships
{
    public class Trade : MonoBehaviour
    {
        [Inject] private List<Harbor> harbors;
        [Inject] private SignalBus signalBus;

        [SerializeField] private Company.CompanyName company;
        [SerializeField] private Arrive arrive;
        [SerializeField] private Seek seek;
        [SerializeField] private Flee flee;

        private Controller controller;
        private Harbor currentHarbor;
        private int currentScore;
        private int levelNumber;
        private bool isDocked;

        private void Start()
        {
            int.TryParse(SceneManager.GetActiveScene().name, out levelNumber);
            controller = GetComponent<Controller>();
            arrive.SetAction(OnArrive);
            Subscribe();
            if (levelNumber >= 9)
                GetBestHarbor();
            else
                LookForHarbors();
        }

        private void Subscribe()
        {
            signalBus.Subscribe<GameEvents.OnCollision>(FishingCollision);
        }

        private void FishingCollision(GameEvents.OnCollision signal)
        {
            if (!signal.collided.Equals(gameObject)) return;
            Death();
        }

        private void LookForHarbors()
        {
            if (harbors == null || harbors.Count == 0) return;
            var availableHarbors = harbors.FindAll(harbor => harbor != currentHarbor);
            if (availableHarbors.Count == 0) return;

            currentHarbor = availableHarbors[Random.Range(0, availableHarbors.Count)];
            SetTarget(currentHarbor.GetDockingPosition());
        }

        private void SetTarget(Transform target)
        {
            seek.SetTarget(target);
            arrive.SetTarget(target);
        }

        private void OnArrive()
        {
            SetTarget(null);
            DOVirtual.DelayedCall(4, () =>
                {
                    if (levelNumber >= 9)
                    {
                        signalBus.Fire(new GameEvents.OnScoreChange{company = company, score = currentScore});
                        GetBestHarbor();
                    }
                    else
                        LookForHarbors();
                }
            );
        }

        public void StartFleeing(Pirate fleeFrom)
        {
            flee.SetTarget(fleeFrom.transform);
            if (!CheckLevel(3)) return;
            FindClosestHarbor();
            controller.SetSpeedLimit(.8f);
        }

        private void FindClosestHarbor()
        {
            if (harbors == null || harbors.Count == 0) return;

            Harbor closestHarbor = null;
            var shortestDistance = Mathf.Infinity;

            foreach (var harbor in harbors)
            {
                var distance = Vector3.Distance(transform.position, harbor.transform.position);
                if (!(distance < shortestDistance)) continue;
                shortestDistance = distance;
                closestHarbor = harbor;
            }

            if (closestHarbor == null) return;
            SetTarget(closestHarbor.transform);
        }

        private void GetBestHarbor()
        {
            Harbor bestHarbor = null;
            var highestScore = int.MinValue;

            foreach (var harbor in harbors)
            {
                if (harbor == currentHarbor) continue;
                var distance = Vector3.Distance(transform.position, harbor.transform.position);
                var score = Mathf.RoundToInt(distance * 0.05f);

                if (score <= highestScore) continue;
                highestScore = score;
                bestHarbor = harbor;
            }

            currentScore = highestScore;
            currentHarbor = bestHarbor;
            if (currentHarbor) SetTarget(currentHarbor.GetDockingPosition());
        }

        public void Death()
        {
            if(levelNumber >= 9)
                signalBus.Fire(new GameEvents.OnTradeShipDestroy {company = company});
            else
            {
                signalBus.Fire(new GameEvents.OnGameEnd());
                Unsubscribe();
                Destroy(gameObject);
            }
        }
        
        private void Unsubscribe()
        {
            signalBus.TryUnsubscribe<GameEvents.OnCollision>(FishingCollision);
        }

        private bool CheckLevel(int activation)
        {
            return levelNumber >= activation;
        }

        public void StopFleeing()
        {
            controller.SetSpeedLimit(1);
            if (levelNumber >= 9)
                GetBestHarbor();
            else
                LookForHarbors();
            flee.SetTarget(null);
        }
    }
}