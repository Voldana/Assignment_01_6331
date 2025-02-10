using UnityEngine;

namespace Project.Scripts.Environment
{
    public class Harbor : MonoBehaviour
    {
        [SerializeField] private Transform dockingPosition;
        [SerializeField] private float destructionRange; 
        [SerializeField] private LayerMask pirateLayer;

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
        
        private void OnDrawGizmos()
        {
            // ✅ Draws the destruction range in the editor for visualization
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f); // Semi-transparent red
            Gizmos.DrawSphere(transform.position, destructionRange);
        }
    }
}