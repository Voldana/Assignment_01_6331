using System;
using System.Collections.Generic;
using System.Linq;
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
            currentHarbor = !currentHarbor
                ? harbors[Random.Range(0, harbors.Capacity)]
                : harbors.Find(harbor => !harbor.Equals(currentHarbor));

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

    }
}