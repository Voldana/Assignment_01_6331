using System.Collections.Generic;
using Project.Scripts.Environment;
using Zenject;

namespace Project.Scripts.Ships
{
    public class Trade : Base
    {
        [Inject] private List<Harbor> harbors;
        
        private bool isDocked;


        private void Start()
        {
            LookForHarbors();
        }

        private void LookForHarbors()
        {
            
        }

        private void Update()
        {
            if (!isDocked)
            {
                Move();
            }
        }
        
    }
}