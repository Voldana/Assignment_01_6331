using UnityEngine;

namespace Project.Scripts.Player
{
    public class ThirdPersonCameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new(0, 5, -10);
        [SerializeField] private float rotationSpeed = 2f;

        private Vector3 currentRotation;

        private void LateUpdate()
        {
            transform.position = target.position + offset;

            if (Input.GetMouseButton(1))
            {
                currentRotation.y += Input.GetAxis("Mouse X") * rotationSpeed;
                currentRotation.x -= Input.GetAxis("Mouse Y") * rotationSpeed;
                currentRotation.x = Mathf.Clamp(currentRotation.x, -20, 60);
            }

            Quaternion rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);
            transform.position = target.position - (rotation * offset);
            transform.LookAt(target);
        }
    }
}