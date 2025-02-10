using System.Collections.Generic;
using DG.Tweening;
using Project.Scripts.Environment;
using Project.Scripts.Steering;
using UnityEngine;
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

        private Harbor currentHarbor;
        private bool isDocked;

        private void Start()
        {
            LookForHarbors();
            arrive.SetAction(OnArrive);
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
        }

        public void StopFleeing()
        {
            flee.SetTarget(null);
        }

    }
}