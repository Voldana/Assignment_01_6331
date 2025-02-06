using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Environment;
using Project.Scripts.Steering;
using UnityEngine;
using Zenject;

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
            seek.SetTarget(currentHarbor.GetDockingPosition());
            arrive.SetTarget(currentHarbor.GetDockingPosition());
        }
        private void Update()
        {
            if (!isDocked)
            {
                
            }
        }

        private void LookForHarbors()
        {
            currentHarbor = !currentHarbor
                ? harbors[Random.Range(0, harbors.Capacity)]
                : harbors.Find(harbor => harbor.Equals(currentHarbor));
        }

    }
}