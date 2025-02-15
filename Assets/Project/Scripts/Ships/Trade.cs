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
    public class Trade: MonoBehaviour
    {
        [Inject] private List<Harbor> harbors;

        [SerializeField] private Arrive arrive;
        [SerializeField] private Seek seek;
        [SerializeField] private Flee flee;

        private Controller controller;
        private Harbor currentHarbor;
        private int levelNumber;
        private bool isDocked;

        private void Start()
        {
            int.TryParse(SceneManager.GetActiveScene().name, out levelNumber);
            controller = GetComponent<Controller>();
            arrive.SetAction(OnArrive);
            LookForHarbors();
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
            DOVirtual.DelayedCall(4, LookForHarbors);   //Waiting for unloading, loading and stuff
            //change score if needed
        }

        public void StartFleeing(Pirate fleeFrom)
        {
            flee.SetTarget(fleeFrom.transform);
            if(!CheckLevel(3)) return;
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
                var distance = Vector3.Distance(transform.position, harbor.GetDockingPosition().position);
                if (!(distance < shortestDistance)) continue;
                shortestDistance = distance;
                closestHarbor = harbor;
            }

            if (closestHarbor == null) return;
            arrive.SetTarget(closestHarbor.GetDockingPosition());
            seek.SetTarget(closestHarbor.GetDockingPosition());
        }

        private bool CheckLevel(int activation)
        {
            return levelNumber >= activation;
        }

        public void StopFleeing()
        {
            controller.SetSpeedLimit(1);
            arrive.SetTarget(currentHarbor.GetDockingPosition());
            seek.SetTarget(currentHarbor.GetDockingPosition());
            flee.SetTarget(null);
        }

    }
}