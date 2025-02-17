using UnityEngine;
using Zenject;

namespace Project.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Inject] private SignalBus signalBus;
        
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float turnSpeed = 100f;

        private bool isUnloading;
        private int score;
        private Vector3 velocity;

        private void Start()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            signalBus.Subscribe<GameEvents.OnCollision>(OnCollision);
        }

        private void OnCollision(GameEvents.OnCollision signal)
        {
            if (!signal.collided.Equals(gameObject)) return;
            Debug.Log("Player collided");
        }

        private void Update()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            velocity = transform.forward * (vertical * moveSpeed * Time.deltaTime);
            transform.position += velocity;

            transform.Rotate(0, horizontal * turnSpeed * Time.deltaTime, 0);
        }

        public Vector3 GetVelocity()
        {
            return velocity;
        }

        private void OnTriggerEnter(Collider other)
        {
           
        }
    }
}