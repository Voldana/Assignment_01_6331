using UnityEngine;

namespace Project.Scripts
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 0.5f;
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float scrollSpeed = 10f;
        [SerializeField] private float minY = 100f;
        [SerializeField] private float maxY = 250f;

        private Vector3 lastMousePosition;

        private void Update()
        {
            Vector3 pos = transform.position;

            // Move with left mouse button
            if (Input.GetMouseButton(0))
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                pos -= new Vector3(delta.x * moveSpeed * Time.deltaTime, 0, delta.y * moveSpeed * Time.deltaTime);
            }

            // Rotate with right mouse button
            if (Input.GetMouseButton(1))
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                transform.Rotate(Vector3.up, delta.x * rotationSpeed * Time.deltaTime, Space.World);
            }

            // Zoom with scroll
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            transform.position = pos;
            lastMousePosition = Input.mousePosition;
        }
    }
}