using Project.Scripts.Steering;
using UnityEngine;

namespace Project.Scripts.Ships
{
    public class Pirate : MonoBehaviour
    {

        [SerializeField] private Pursue pursue;
        [SerializeField] private Wander wander;
        

        public void StartChasing(GameObject target)
        {
            wander.SetStatus(false);
            pursue.SetTarget(target.transform);
        }

        public void StopChasing()
        {
            wander.SetStatus(true);
            pursue.SetTarget(null);
        }
    }
}