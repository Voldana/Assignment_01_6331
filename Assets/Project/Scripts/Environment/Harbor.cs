using UnityEngine;

namespace Project.Scripts.Environment
{
    public class Harbor : MonoBehaviour
    {
        [SerializeField] private Transform dockingPosition;

        private bool isFull;

        public bool IsFull()
        {
            return isFull;
        }

        public void SetFull(bool status)
        {
            isFull = status;
        }

        public Transform GetDockingPosition()
        {
            return dockingPosition;
        }
    }
}